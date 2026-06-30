using System;

namespace Calcpad.Core
{
    // Parte NO generada: inyecta el JS de graficas INLINE en el HTML, reemplazando los
    // <script src="https://calcpad.local/{mlplot,glplot}.js"></script> que emiten los
    // macros. La 1a aparicion se sustituye por el JS embebido; las siguientes se borran
    // (la libreria se carga una sola vez). Asi el HTML es 100% autocontenido: no depende
    // de archivos doc\*.js ni del host virtual calcpad.local.
    internal static partial class EmbeddedGraphics
    {
        public static string Inline(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;
            html = InlineOne(html, "https://calcpad.local/mlplot.js", MlplotJs);
            html = InlineOne(html, "https://calcpad.local/glplot.js", GlplotJs);
            return html;
        }

        private static string InlineOne(string html, string url, string js)
        {
            var tag = "<script src=\"" + url + "\"></script>";
            var i = html.IndexOf(tag, StringComparison.Ordinal);
            if (i < 0) return html;
            // 1a aparicion -> JS inline ; resto -> se elimina (cargar la libreria una vez)
            var rest = html.Substring(i + tag.Length).Replace(tag, string.Empty);
            return html.Substring(0, i) + "<script>\n" + js + "\n</script>" + rest;
        }
    }
}
