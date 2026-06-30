// AUTO-GENERADO desde Calcpad-FEM-3D/{mlplot.js,glplot.js,mlplot.cpd}.
// Libreria de graficas estilo MATLAB EMBEBIDA en el codigo fuente (sin archivos doc separados).
// Regenerar:  python Tools/gen_embedded_graphics.py
namespace Calcpad.Core
{
    internal static partial class EmbeddedGraphics
    {
        public const string GlplotJs = """"
/* glplot.js — motor 3D REAL con WebGL crudo sobre <canvas> (sin three.js, sin CDN).
   GPU, depth-test, perspectiva, orbita con el mouse. fill3 = caras; line3 = lineas
   (malla sin deformar, apoyos, flecha de carga, caja de ejes). Embebible inline en figure3$. */
(function (global) {
  var GL3 = {
    st: null,
    etabs: false,   // true = paleta de bandas DISCRETAS de ETABS; false = jet_r suave
    // paleta de contorno de ETABS — INGENIERIA INVERSA (15 bandas muestreadas de la
    // leyenda real, capture_20260624_174436): magenta(min) -> rojo -> amarillo -> verde -> azul(max)
    pal: [[0.784,0,0.784],[0.894,0,0.392],[1,0,0],[1,0.251,0],[1,0.502,0],[1,0.667,0],
          [1,0.831,0],[1,1,0],[0.502,1,0],[0,1,0],[0,1,0.502],[0,1,1],[0,0.667,1],[0,0.333,1],[0,0,1]],
    _ins: function (el) {                       // inserta un elemento donde esta el <script> actual
      var sc = document.currentScript;
      if (sc && sc.parentNode) sc.parentNode.insertBefore(el, sc); else document.body.appendChild(el);
    },
    figure3: function (id, w, h) {              // el .js CREA el canvas WebGL + un overlay 2D para el texto
      var wrap = document.createElement("div");
      wrap.style.cssText = "position:relative;display:inline-block;vertical-align:top;margin:4px";
      var cv = document.createElement("canvas"); cv.id = id; cv.style.cssText = "display:block;border:1px solid #ccc";
      var ov = document.createElement("canvas"); ov.style.cssText = "position:absolute;left:0;top:0;pointer-events:none";
      var tip = document.createElement("div");   // datatip tipo MATLAB (sigue al cursor, sobre la malla)
      tip.style.cssText = "position:absolute;pointer-events:none;display:none;background:rgba(25,28,32,0.9);color:#fff;font:11px Segoe UI;padding:3px 7px;border-radius:4px;white-space:nowrap;transform:translate(12px,-50%);z-index:5;box-shadow:0 1px 4px rgba(0,0,0,0.3)";
      wrap.appendChild(cv); wrap.appendChild(ov); wrap.appendChild(tip);
      var row = document.createElement("div");   // fila flex: canvas + colorbar SIEMPRE en la misma línea
      row.style.cssText = "display:flex;align-items:flex-start;flex-wrap:nowrap";
      this._ins(row); row.appendChild(wrap);
      var dpr = Math.max(2, window.devicePixelRatio || 1);   // render a >=2x para que no pixele al acercar
      cv.width = w * dpr; cv.height = h * dpr; cv.style.width = w + "px"; cv.style.height = h + "px";
      ov.width = w * dpr; ov.height = h * dpr; ov.style.width = w + "px"; ov.style.height = h + "px";
      var octx = ov.getContext("2d"); octx.scale(dpr, dpr);
      var opt = { preserveDrawingBuffer: true, antialias: true };
      var gl = cv.getContext("webgl", opt) || cv.getContext("experimental-webgl", opt);
      this.st = { gl: gl, cv: cv, octx: octx, w: w, h: h, row: row, verts: [], lverts: [], ticks: [], dpts: [], tip: tip, tipOn: false, tipLabel: "", az: 0.8, el: 0.5, bb: [0, 1, 0, 1, 0, 1], dist: 3 };
      gl.viewport(0, 0, cv.width, cv.height); gl.enable(gl.DEPTH_TEST); gl.clearColor(1, 1, 1, 1);
    },
    tick3: function (x, y, z, t) { this.st.ticks.push({ x: x, y: y, z: z, t: "" + t }); },   // texto 3D (numero/etiqueta) que sigue la rotacion
    datatip: function (label) { this.st.tipOn = true; this.st.tipLabel = "" + label; },      // activa el datatip (hover) tipo MATLAB datacursormode
    datapoint: function (x, y, z, v) { this.st.dpts.push({ x: x, y: y, z: z, v: v }); },      // registra un nudo y su valor para el hover
    cartesian3: function (x0, x1, y0, y1, z0, z1, ndx, ndz) {   // plano cartesiano 3D: caja + numeros X/Z (1 llamada)
      var L = this.st.lverts, c = this.hex("cfd4d8"), i, dx = x1 - x0, dz = z1 - z0;
      function ln(ax, ay, az, bx, by, bz) { L.push(ax, ay, az, c[0], c[1], c[2], bx, by, bz, c[0], c[1], c[2]); }
      ln(x0, y0, z0, x1, y0, z0); ln(x1, y0, z0, x1, y1, z0); ln(x1, y1, z0, x0, y1, z0); ln(x0, y1, z0, x0, y0, z0);
      ln(x0, y0, z1, x1, y0, z1); ln(x1, y0, z1, x1, y1, z1); ln(x1, y1, z1, x0, y1, z1); ln(x0, y1, z1, x0, y0, z1);
      ln(x0, y0, z0, x0, y0, z1); ln(x1, y0, z0, x1, y0, z1); ln(x1, y1, z0, x1, y1, z1); ln(x0, y1, z0, x0, y1, z1);
      for (i = 0; i <= ndx; i++) { var xv = x0 + i * dx / ndx; this.tick3(xv, y0, z0 - dz * 0.05, Math.round(xv)); }
      for (i = 0; i <= ndz; i++) { var zv = z0 + i * dz / ndz; this.tick3(x0 - dx * 0.06, y0, zv, Math.round(zv)); }
      this.tick3((x0 + x1) / 2, y0, z0 - dz * 0.13, "X (ancho, m)");
      this.tick3(x0 - dx * 0.15, y0, (z0 + z1) / 2, "Z (altura, m)");
    },
    _proj2: function (s, x, y, z) {              // proyecta un punto 3D a pixeles de pantalla (con la MVP actual)
      var m = s.mvp, cw = m[3] * x + m[7] * y + m[11] * z + m[15];
      var cx = m[0] * x + m[4] * y + m[8] * z + m[12], cy = m[1] * x + m[5] * y + m[9] * z + m[13];
      return [(cx / cw * 0.5 + 0.5) * s.w, (1 - (cy / cw * 0.5 + 0.5)) * s.h];
    },
    _nf: function (v) {                          // formato: notacion cientifica para valores muy chicos/grandes
      var a = Math.abs(v);
      if (a < 1e-12) return "0";
      if (a >= 1e4 || a < 1e-3) return v.toExponential(2);
      if (a < 1) return parseFloat(v.toPrecision(3)).toString();
      return v.toFixed(2);
    },
    colorbar3: function (vmin, vmax, h) {        // el .js CREA el <div> de la barra de color
      var d = document.createElement("div");
      d.style.cssText = "display:inline-flex;vertical-align:top;margin:6px 0 0 10px;font:11px Segoe UI;color:#333";
      function rgb(c){ return "rgb(" + Math.round(c[0]*255) + "," + Math.round(c[1]*255) + "," + Math.round(c[2]*255) + ")"; }
      var grad, lab = "", k, i;
      if (this.etabs) {                          // BANDAS DISCRETAS con la paleta ETABS (15 colores)
        var N = 15, stops = [];
        for (i = 0; i < N; i++) {                 // cada banda = color solido de la paleta, con corte duro
          var col = rgb(this.pal[i]), p0 = (i / N * 100).toFixed(2), p1 = ((i + 1) / N * 100).toFixed(2);
          stops.push(col + " " + p0 + "%", col + " " + p1 + "%");
        }
        grad = "linear-gradient(to top," + stops.join(",") + ")";
        for (k = N; k >= 0; k--) lab += "<span>" + this._nf(vmin + k / N * (vmax - vmin)) + "</span>";
      } else {                                    // jet suave (didactico)
        grad = "linear-gradient(to top,rgb(128,0,0),rgb(255,0,0),rgb(255,128,0),rgb(255,255,0),rgb(120,255,120),rgb(0,220,255),rgb(0,120,255),rgb(0,0,255),rgb(0,0,140))";
        for (k = 4; k >= 0; k--) lab += "<span>" + this._nf(vmin + k / 4 * (vmax - vmin)) + "</span>";
      }
      d.innerHTML = '<div style="width:18px;height:' + h + 'px;background:' + grad + ';border:1px solid #444"></div>'
        + '<div style="display:flex;flex-direction:column;justify-content:space-between;height:' + h + 'px;margin-left:5px">' + lab + '</div>';
      if (this.st && this.st.row) this.st.row.appendChild(d); else this._ins(d);   // misma fila que el canvas
    },
    view3: function (azd, eld) { this.st.az = azd * 0.0174533; this.st.el = eld * 0.0174533; },
    axis3: function (x0, x1, y0, y1, z0, z1) {
      this.st.bb = [x0, x1, y0, y1, z0, z1]; this.st.verts = []; this.st.lverts = []; this.st.ticks = []; this.st.dpts = []; this.st.tipOn = false;
      var dx = x1 - x0, dy = y1 - y0, dz = z1 - z0; this.st.dist = 2.1 * Math.sqrt(dx * dx + dy * dy + dz * dz);
    },
    jet: function (t) {
      t = Math.max(0, Math.min(1, t)); var u = 1 - t;
      function c(z) { return Math.max(0, Math.min(1, 1.5 - Math.abs(4 * u - z))); }
      return [c(3), c(2), c(1)];
    },
    hex: function (s) {
      if (s.charCodeAt(0) === 35) s = s.substr(1);
      if (s.length === 3) s = s[0] + s[0] + s[1] + s[1] + s[2] + s[2];
      return [parseInt(s.substr(0, 2), 16) / 255, parseInt(s.substr(2, 2), 16) / 255, parseInt(s.substr(4, 2), 16) / 255];
    },
    fill3: function (p, t1, t2, t3, t4) {        // guarda el ESCALAR t por vertice (el shader hace jet suave O bandas ETABS)
      if (t2 === undefined) { t2 = t1; t3 = t1; t4 = t1; }
      var tt = [t1, t2, t3, t4], idx = [0, 1, 2, 0, 2, 3], i, k, t, V = this.st.verts;
      for (i = 0; i < 6; i++) { k = idx[i]; t = tt[k]; V.push(p[3 * k], p[3 * k + 1], p[3 * k + 2], t, t, t); }
    },
    line3: function (x1, y1, z1, x2, y2, z2, col) {
      var c = this.hex(col), L = this.st.lverts;
      L.push(x1, y1, z1, c[0], c[1], c[2], x2, y2, z2, c[0], c[1], c[2]);
    },
    point3: function (x, y, z, t, sz) {          // scatter3: marcador 3D = 3 quads cruzados (visible desde cualquier angulo)
      var b = this.st.bb, d = Math.max(b[1] - b[0], b[3] - b[2], b[5] - b[4]) * (sz || 0.015);
      this.fill3([x - d, y - d, z, x + d, y - d, z, x + d, y + d, z, x - d, y + d, z], t, t, t, t);  // plano XY
      this.fill3([x - d, y, z - d, x + d, y, z - d, x + d, y, z + d, x - d, y, z + d], t, t, t, t);  // plano XZ
      this.fill3([x, y - d, z - d, x, y + d, z - d, x, y + d, z + d, x, y - d, z + d], t, t, t, t);  // plano YZ
    },
    quiver3: function (x, y, z, dx, dy, dz, col) {  // vector 3D: tallo + cabeza de flecha (2 lineas)
      this.line3(x, y, z, x + dx, y + dy, z + dz, col);
      var L = Math.sqrt(dx * dx + dy * dy + dz * dz) || 1, ux = dx / L, uy = dy / L, uz = dz / L, h = L * 0.22;
      var ax = x + dx, ay = y + dy, az = z + dz, px = -uy, py = ux, pz = 0, pl = Math.sqrt(px * px + py * py + pz * pz);
      if (pl < 1e-6) { px = 0; py = -uz; pz = uy; pl = Math.sqrt(py * py + pz * pz) || 1; } px /= pl; py /= pl; pz /= pl;
      this.line3(ax, ay, az, ax - ux * h + px * h * 0.5, ay - uy * h + py * h * 0.5, az - uz * h + pz * h * 0.5, col);
      this.line3(ax, ay, az, ax - ux * h - px * h * 0.5, ay - uy * h - py * h * 0.5, az - uz * h - pz * h * 0.5, col);
    },
    stem3: function (x, y, z, t) {               // tallo 3D vertical (z=0 -> z) + marcador
      this.line3(x, y, 0, x, y, z, "808080"); this.point3(x, y, z, t);
    },
    tri3: function (x1, y1, z1, x2, y2, z2, x3, y3, z3, t1, t2, t3) {  // trisurf/trimesh: cara TRIANGULAR (4o vertice = 3o, degenerado)
      this.fill3([x1, y1, z1, x2, y2, z2, x3, y3, z3, x3, y3, z3], t1, t2, t3 === undefined ? t1 : t3, t3 === undefined ? t1 : t3);
    },
    sphere: function (cx, cy, cz, r, t, nu, nv) {  // esfera completa (loops lat/long -> fill3)
      nu = nu || 16; nv = nv || 10; var i, j, GL = this;
      function P(u, v) { var th = u / nu * 6.28318, ph = v / nv * 3.14159; return [cx + r * Math.sin(ph) * Math.cos(th), cy + r * Math.sin(ph) * Math.sin(th), cz + r * Math.cos(ph)]; }
      for (j = 0; j < nv; j++) for (i = 0; i < nu; i++) { var a = P(i, j), b = P(i + 1, j), c = P(i + 1, j + 1), d = P(i, j + 1); GL.fill3([a[0], a[1], a[2], b[0], b[1], b[2], c[0], c[1], c[2], d[0], d[1], d[2]], t, t, t, t); }
    },
    cylinder: function (cx, cy, z0, z1, r, t, nu) {  // cilindro (cara lateral) -> fill3
      nu = nu || 20; var i; for (i = 0; i < nu; i++) { var a0 = i / nu * 6.28318, a1 = (i + 1) / nu * 6.28318, x0 = cx + r * Math.cos(a0), y0 = cy + r * Math.sin(a0), x1 = cx + r * Math.cos(a1), y1 = cy + r * Math.sin(a1); this.fill3([x0, y0, z0, x1, y1, z0, x1, y1, z1, x0, y0, z1], t, t, t, t); }
    },
    light: false,                                          // iluminacion 3D (GL3.lighting / lighting$ en el .cpd)
    lighting: function (on) { this.light = !(on === 0 || on === "off" || on === "none" || on === false); },
    shading: function (m) { this.light = !(m === "none" || m === "off" || m === 0); },   // flat/interp -> con luz
    render3: function () {
      var s = this.st, gl = s.gl, GL3 = this;
      gl.getExtension("OES_standard_derivatives");          // normal por derivadas de pantalla (para iluminar)
      var vs = "attribute vec3 a;attribute vec3 c;uniform mat4 m;varying vec3 v;varying vec3 vp;void main(){gl_Position=m*vec4(a,1.0);v=c;vp=a;}";
      // uMode: 0=jet suave · 1=bandas DISCRETAS con la paleta ETABS · 2=linea (rgb directo). uLight: 0/1 iluminacion difusa.
      var fs = "#extension GL_OES_standard_derivatives : enable\n"
        + "precision mediump float;varying vec3 v;varying vec3 vp;uniform float uMode;uniform float uLight;uniform vec3 pal[15];"
        + "void main(){"
        + " if(uMode>1.5){gl_FragColor=vec4(v,1.0);return;}" // lineas: rgb directo, sin luz
        + " float t=clamp(v.r,0.0,1.0); vec3 col;"
        + " if(uMode>0.5){"                                  // BANDAS ETABS: indexar la paleta de 15 colores
        + "   int bi=int(clamp(floor(t*15.0),0.0,14.0)); col=pal[0];"
        + "   for(int i=0;i<15;i++){ if(i<=bi) col=pal[i]; } }"
        + " else { float u=1.0-t;"                           // jet suave (didactico)
        + "   col=vec3(clamp(1.5-abs(4.0*u-3.0),0.0,1.0),clamp(1.5-abs(4.0*u-2.0),0.0,1.0),clamp(1.5-abs(4.0*u-1.0),0.0,1.0)); }"
        + " if(uLight>0.5){"                                 // luz difusa: normal de la cara = cross de las derivadas
        + "   vec3 N=normalize(cross(dFdx(vp),dFdy(vp))); vec3 L=normalize(vec3(0.4,0.5,0.85));"
        + "   col=col*(0.55+0.45*abs(dot(N,L))); }"          // dos caras (abs): ambiente 0.55 + difuso 0.45
        + " gl_FragColor=vec4(col,1.0);}";
      function sh(t, src) { var o = gl.createShader(t); gl.shaderSource(o, src); gl.compileShader(o); return o; }
      var pr = gl.createProgram();
      gl.attachShader(pr, sh(gl.VERTEX_SHADER, vs)); gl.attachShader(pr, sh(gl.FRAGMENT_SHADER, fs));
      gl.linkProgram(pr); gl.useProgram(pr); s.pr = pr;
      s.um = gl.getUniformLocation(pr, "m"); s.la = gl.getAttribLocation(pr, "a"); s.lc = gl.getAttribLocation(pr, "c");
      s.umode = gl.getUniformLocation(pr, "uMode"); s.ulight = gl.getUniformLocation(pr, "uLight");
      var P = this.pal, pf = []; for (var pi = 0; pi < 15; pi++) { pf.push(P[pi][0], P[pi][1], P[pi][2]); }
      gl.uniform3fv(gl.getUniformLocation(pr, "pal"), new Float32Array(pf));
      s.tb = gl.createBuffer(); gl.bindBuffer(gl.ARRAY_BUFFER, s.tb); gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(s.verts), gl.STATIC_DRAW); s.nv = s.verts.length / 6;
      s.lb = gl.createBuffer(); gl.bindBuffer(gl.ARRAY_BUFFER, s.lb); gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(s.lverts), gl.STATIC_DRAW); s.nl = s.lverts.length / 6;
      var draw = function () { GL3._draw(s); }; s.draw = draw; draw();
      var cv = s.cv, drag = false, panning = false, px = 0, py = 0; cv.style.cursor = "grab";
      cv.oncontextmenu = function (e) { e.preventDefault(); };   // permitir arrastrar con boton derecho (pan)
      cv.onmousedown = function (e) { drag = true; panning = e.shiftKey || e.button === 2; px = e.clientX; py = e.clientY; cv.style.cursor = panning ? "move" : "grabbing"; e.preventDefault(); };
      cv.onmousemove = function (e) {
        if (drag && panning) {                        // PAN (shift o boton derecho): desplaza el centro en pantalla
          s.panx = (s.panx || 0) + (e.clientX - px) * s.dist * 0.0022; s.pany = (s.pany || 0) - (e.clientY - py) * s.dist * 0.0022;
          px = e.clientX; py = e.clientY; draw(); return;
        }
        if (drag) {                                   // TRACKBALL: rotación incremental en TODOS los sentidos
          var dx = (e.clientX - px) * 0.01, dy = (e.clientY - py) * 0.01; px = e.clientX; py = e.clientY;
          var dR = GL3._mul(GL3._rotX(dy), GL3._rotY(dx));
          s.rot = GL3._mul(dR, s.rot); draw(); return;
        }
        if (!s.tipOn || !s.dpts.length || !s.mvp) return;   // hover: nudo más cercano al cursor → datatip
        var r = cv.getBoundingClientRect(), mx = e.clientX - r.left, my = e.clientY - r.top, best = -1, bd = 324, i, p, dx, dy;
        for (i = 0; i < s.dpts.length; i++) { p = GL3._proj2(s, s.dpts[i].x, s.dpts[i].y, s.dpts[i].z); dx = p[0] - mx; dy = p[1] - my; if (dx * dx + dy * dy < bd) { bd = dx * dx + dy * dy; best = i; } }
        if (best >= 0) { var dp = s.dpts[best], pp = GL3._proj2(s, dp.x, dp.y, dp.z); s.tip.style.display = "block"; s.tip.style.left = pp[0] + "px"; s.tip.style.top = pp[1] + "px"; s.tip.innerHTML = (s.tipLabel ? s.tipLabel + " = " : "") + GL3._nf(dp.v); }
        else s.tip.style.display = "none";
      };
      var up = function () { drag = false; cv.style.cursor = "grab"; if (s.tip) s.tip.style.display = "none"; };
      cv.onmouseup = up; cv.onmouseleave = up;
      cv.onwheel = function (e) { s.dist *= e.deltaY > 0 ? 1.1 : 0.9; draw(); e.preventDefault(); };
    },
    _attr: function (s, b) {
      var gl = s.gl; gl.bindBuffer(gl.ARRAY_BUFFER, b);
      gl.enableVertexAttribArray(s.la); gl.vertexAttribPointer(s.la, 3, gl.FLOAT, false, 24, 0);
      gl.enableVertexAttribArray(s.lc); gl.vertexAttribPointer(s.lc, 3, gl.FLOAT, false, 24, 12);
    },
    _draw: function (s) {
      var gl = s.gl; gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
      var b = s.bb, cx = (b[0] + b[1]) / 2, cy = (b[2] + b[3]) / 2, cz = (b[4] + b[5]) / 2;
      // TRACKBALL: rotación libre acumulada en s.rot (todos los sentidos). V = pull-back · rot · centrar.
      if (!s.rot) s.rot = this._mul(this._rotZ(-0.6), this._rotX(-1.25));   // vista iso inicial (modelo Z-up)
      var V = this._mul(this._trans(s.panx || 0, s.pany || 0, -s.dist), this._mul(s.rot, this._trans(-cx, -cy, -cz)));
      var P = this._persp(0.8, s.w / s.h, 0.01, s.dist * 12);
      s.mvp = this._mul(P, V);
      gl.uniformMatrix4fv(s.um, false, s.mvp);
      gl.uniform1f(s.umode, GL3.etabs ? 1 : 0);                 // caras: jet suave o bandas ETABS
      gl.uniform1f(s.ulight, GL3.light ? 1 : 0);                // iluminacion difusa on/off
      this._attr(s, s.tb); gl.drawArrays(gl.TRIANGLES, 0, s.nv);
      if (s.nl > 0) { gl.uniform1f(s.umode, 2); this._attr(s, s.lb); gl.drawArrays(gl.LINES, 0, s.nl); }  // lineas: rgb directo
      var oc = s.octx; oc.clearRect(0, 0, s.w, s.h);
      if (s.ticks.length) {
        oc.fillStyle = "#444"; oc.font = "11px Segoe UI"; oc.textAlign = "center"; oc.textBaseline = "middle";
        for (var i = 0; i < s.ticks.length; i++) { var k = s.ticks[i], p = this._proj2(s, k.x, k.y, k.z); oc.fillText(k.t, p[0], p[1]); }
      }
    },
    _persp: function (fy, a, n, f) {
      var t = 1 / Math.tan(fy / 2), nf = 1 / (n - f);
      return [t / a, 0, 0, 0, 0, t, 0, 0, 0, 0, (f + n) * nf, -1, 0, 0, 2 * f * n * nf, 0];
    },
    _look: function (e, c, u) {
      var z0 = e[0] - c[0], z1 = e[1] - c[1], z2 = e[2] - c[2], zl = 1 / Math.sqrt(z0 * z0 + z1 * z1 + z2 * z2);
      z0 *= zl; z1 *= zl; z2 *= zl;
      var x0 = u[1] * z2 - u[2] * z1, x1 = u[2] * z0 - u[0] * z2, x2 = u[0] * z1 - u[1] * z0, xl = Math.sqrt(x0 * x0 + x1 * x1 + x2 * x2);
      if (xl) { xl = 1 / xl; x0 *= xl; x1 *= xl; x2 *= xl; }
      var y0 = z1 * x2 - z2 * x1, y1 = z2 * x0 - z0 * x2, y2 = z0 * x1 - z1 * x0;
      return [x0, y0, z0, 0, x1, y1, z1, 0, x2, y2, z2, 0, -(x0 * e[0] + x1 * e[1] + x2 * e[2]), -(y0 * e[0] + y1 * e[1] + y2 * e[2]), -(z0 * e[0] + z1 * e[1] + z2 * e[2]), 1];
    },
    _mul: function (a, b) {
      var o = new Array(16), c, r, k;
      for (c = 0; c < 4; c++) for (r = 0; r < 4; r++) { var s = 0; for (k = 0; k < 4; k++) s += a[k * 4 + r] * b[c * 4 + k]; o[c * 4 + r] = s; }
      return o;
    },
    _trans: function (x, y, z) { return [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, y, z, 1]; },          // column-major
    _rotX: function (a) { var c = Math.cos(a), s = Math.sin(a); return [1, 0, 0, 0, 0, c, s, 0, 0, -s, c, 0, 0, 0, 0, 1]; },
    _rotY: function (a) { var c = Math.cos(a), s = Math.sin(a); return [c, 0, -s, 0, 0, 1, 0, 0, s, 0, c, 0, 0, 0, 0, 1]; },
    _rotZ: function (a) { var c = Math.cos(a), s = Math.sin(a); return [c, s, 0, 0, -s, c, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1]; }
  };
  global.GL3 = GL3;
})(window);
"""";

        public const string MlplotJs = """"
/* mlplot.js — motor de graficas estilo MATLAB para Calcpad.
   Todo el dibujo canvas vive aqui. Los .cpd solo llaman ML.* (sin <> ).
   2D: mapeo DATOS->pixeles via ML.axis. 3D: ML.axis3 + arrastrar para ORBITAR. Colormap = jet_r. */
(function (global) {
  var ML = {
    figs: {}, cur: null,

    figure: function (id, w, h) {              // el .js CREA el canvas (no hay <canvas> en el .cpd)
      var wrap = document.createElement("div");
      wrap.style.cssText = "position:relative;display:inline-block;vertical-align:top;margin:4px";
      var cv = document.createElement("canvas"); cv.id = id;
      cv.style.cssText = "display:block;background:#fff;border:1px solid #ccc;width:" + w + "px;height:" + h + "px";
      var tip = document.createElement("div");   // datatip 2D (hover) que sigue al cursor
      tip.style.cssText = "position:absolute;pointer-events:none;display:none;background:rgba(25,28,32,0.9);color:#fff;font:11px Segoe UI;padding:3px 7px;border-radius:4px;white-space:nowrap;transform:translate(10px,-50%);z-index:5;box-shadow:0 1px 4px rgba(0,0,0,0.3)";
      wrap.appendChild(cv); wrap.appendChild(tip);
      var sc = document.currentScript;
      if (sc && sc.parentNode) sc.parentNode.insertBefore(wrap, sc); else document.body.appendChild(wrap);
      var dpr = Math.max(2, window.devicePixelRatio || 1);   // render a >=2x para que no pixele al acercar
      cv.width = w * dpr; cv.height = h * dpr;
      var ctx = cv.getContext("2d");
      ctx.scale(dpr, dpr);
      ctx.lineJoin = "round"; ctx.lineCap = "round";
      ctx.font = "11px Segoe UI";
      this.figs[id] = { ctx: ctx, cv: cv, tip: tip, dpr: dpr, w: w, h: h, xmin: 0, xmax: w, ymin: 0, ymax: h,
                        sc: 1, padx: 8, pady: 8, mode: "2d", az: 0.7, el: 0.45, prims: [], dpts: [], tipOn: false, tipLabel: "" };
      this.cur = id;
    },
    S: function () { return this.figs[this.cur]; },
    hold: function () { },

    // ===================== 2D =====================
    axis: function (x0, x1, y0, y1) {            // EQUAL-aspect (geometria / FEM)
      var s = this.S(); s.mode = "2d";
      s.xmin = x0; s.xmax = x1; s.ymin = y0; s.ymax = y1;
      s.mL = 38; s.mB = 26; s.mR = 84; s.mT = 14;   // margenes: izq(numeros y), abajo(numeros x), der(colorbar), arriba
      var aW = s.w - s.mL - s.mR, aH = s.h - s.mT - s.mB;
      s.sc = Math.min(aW / (x1 - x0), aH / (y1 - y0));
      s.scx = s.sc; s.scy = s.sc; s.bx0 = x0; s.by0 = y0; s.xlog = 0; s.ylog = 0;
    },
    axischart: function (x0, x1, y0, y1) {       // escala INDEPENDIENTE (datos: estira para llenar)
      var s = this.S(); s.mode = "2d";
      s.xmin = x0; s.xmax = x1; s.ymin = y0; s.ymax = y1; s.xlog = 0; s.ylog = 0;
      s.mL = 46; s.mB = 28; s.mR = 18; s.mT = 16;
      s.scx = (s.w - s.mL - s.mR) / (x1 - x0); s.scy = (s.h - s.mT - s.mB) / (y1 - y0);
      s.sc = Math.min(s.scx, s.scy); s.bx0 = x0; s.by0 = y0;
    },
    axislog: function (x0, x1, y0, y1, xl, yl) { // ejes log (semilogx/semilogy/loglog)
      this.axischart(x0, x1, y0, y1); var s = this.S(); s.xlog = xl ? 1 : 0; s.ylog = yl ? 1 : 0;
      s.bx0 = s.xlog ? Math.log10(x0) : x0; s.by0 = s.ylog ? Math.log10(y0) : y0;
      s.scx = (s.w - s.mL - s.mR) / ((s.xlog ? Math.log10(x1) : x1) - s.bx0);
      s.scy = (s.h - s.mT - s.mB) / ((s.ylog ? Math.log10(y1) : y1) - s.by0);
    },
    X: function (x) { var s = this.S(); return s.mL + ((s.xlog ? Math.log10(x) : x) - s.bx0) * s.scx; },
    Y: function (y) { var s = this.S(); return s.h - s.mB - ((s.ylog ? Math.log10(y) : y) - s.by0) * s.scy; },
    grid: function (ndx, ndy) {                  // plano cartesiano 2D: grilla + ticks + numeros + caja
      var s = this.S(), ctx = s.ctx, i;
      ctx.lineWidth = 1; ctx.font = "10px Segoe UI";
      for (i = 0; i <= ndx; i++) {
        var xv = s.xmin + i * (s.xmax - s.xmin) / ndx, px = this.X(xv);
        ctx.strokeStyle = "rgba(0,0,0,0.06)"; ctx.beginPath(); ctx.moveTo(px, this.Y(s.ymax)); ctx.lineTo(px, this.Y(s.ymin)); ctx.stroke();
        ctx.fillStyle = "#555"; ctx.textAlign = "center"; ctx.textBaseline = "top"; ctx.fillText(this._fmt(xv), px, this.Y(s.ymin) + 4);
      }
      for (i = 0; i <= ndy; i++) {
        var yv = s.ymin + i * (s.ymax - s.ymin) / ndy, py = this.Y(yv);
        ctx.strokeStyle = "rgba(0,0,0,0.06)"; ctx.beginPath(); ctx.moveTo(this.X(s.xmin), py); ctx.lineTo(this.X(s.xmax), py); ctx.stroke();
        ctx.fillStyle = "#555"; ctx.textAlign = "right"; ctx.textBaseline = "middle"; ctx.fillText(this._fmt(yv), this.X(s.xmin) - 5, py);
      }
      ctx.strokeStyle = "#888"; ctx.lineWidth = 1;
      ctx.strokeRect(this.X(s.xmin), this.Y(s.ymax), this.X(s.xmax) - this.X(s.xmin), this.Y(s.ymin) - this.Y(s.ymax));
    },
    _fmt: function (v) { return Math.abs(v) < 1e-9 ? "0" : (v === Math.round(v) ? v.toFixed(0) : v.toFixed(1)); },
    _nf: function (v) { var a = Math.abs(v); if (a < 1e-12) return "0"; if (a >= 1e4 || a < 1e-3) return v.toExponential(2); if (a < 1) return parseFloat(v.toPrecision(3)).toString(); return v.toFixed(2); },
    datatip: function (label) {                  // datatip 2D (hover) estilo MATLAB datacursormode
      var s = this.S(), ML = this; s.tipOn = true; s.tipLabel = "" + label;
      if (s._tipBound) return; s._tipBound = true;
      s.cv.onmousemove = function (e) {
        if (!s.tipOn || !s.dpts.length) return;
        var r = s.cv.getBoundingClientRect(), mx = e.clientX - r.left, my = e.clientY - r.top, best = -1, bd = 289, i, px, py, dx, dy;
        for (i = 0; i < s.dpts.length; i++) { var d = s.dpts[i]; px = ML.X(d.x); py = ML.Y(d.y); dx = px - mx; dy = py - my; if (dx * dx + dy * dy < bd) { bd = dx * dx + dy * dy; best = i; } }
        if (best >= 0) { var d = s.dpts[best], qx = ML.X(d.x), qy = ML.Y(d.y); s.tip.style.display = "block"; s.tip.style.left = qx + "px"; s.tip.style.top = qy + "px"; s.tip.innerHTML = (s.tipLabel ? s.tipLabel + " = " : "") + ML._nf(d.v); }
        else s.tip.style.display = "none";
      };
      s.cv.onmouseleave = function () { s.tip.style.display = "none"; };
    },
    datapoint: function (x, y, v) { this.S().dpts.push({ x: x, y: y, v: v }); },
    jet: function (t) {
      t = Math.max(0, Math.min(1, t)); var u = 1 - t;
      function c(z) { return Math.round(255 * Math.max(0, Math.min(1, 1.5 - Math.abs(4 * u - z)))); }
      return "rgb(" + c(3) + "," + c(2) + "," + c(1) + ")";
    },
    patch: function (p, t) {
      var ctx = this.S().ctx; ctx.beginPath(); ctx.moveTo(this.X(p[0]), this.Y(p[1]));
      for (var i = 2; i < p.length; i += 2) ctx.lineTo(this.X(p[i]), this.Y(p[i + 1]));
      ctx.closePath(); ctx.fillStyle = this.jet(t); ctx.fill();
      ctx.strokeStyle = "rgba(0,0,0,0.18)"; ctx.lineWidth = 0.5; ctx.stroke();
    },
    fill: function (p, col) {
      var ctx = this.S().ctx; ctx.beginPath(); ctx.moveTo(this.X(p[0]), this.Y(p[1]));
      for (var i = 2; i < p.length; i += 2) ctx.lineTo(this.X(p[i]), this.Y(p[i + 1]));
      ctx.closePath(); ctx.fillStyle = col; ctx.fill();
      ctx.strokeStyle = "#888"; ctx.lineWidth = 1; ctx.stroke();
    },
    line: function (x1, y1, x2, y2, col) {
      var ctx = this.S().ctx; ctx.strokeStyle = col; ctx.lineWidth = 1.5;
      ctx.beginPath(); ctx.moveTo(this.X(x1), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y2)); ctx.stroke();
    },
    plot: function (x1, y1, x2, y2, col) {
      var ctx = this.S().ctx; ctx.strokeStyle = col; ctx.lineWidth = 2;
      ctx.beginPath(); ctx.moveTo(this.X(x1), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y2)); ctx.stroke();
    },
    scatter: function (x, y, col) {
      var ctx = this.S().ctx; ctx.fillStyle = col; ctx.beginPath(); ctx.arc(this.X(x), this.Y(y), 4, 0, 6.2832); ctx.fill();
    },
    rectangle: function (x1, y1, x2, y2, col) {
      var ctx = this.S().ctx; ctx.strokeStyle = col; ctx.lineWidth = 1.5;
      ctx.strokeRect(this.X(x1), this.Y(y1), this.X(x2) - this.X(x1), this.Y(y2) - this.Y(y1));
    },
    fixed: function (x1, y, x2) {                  // apoyo EMPOTRADO: linea + rayado de tierra
      var ctx = this.S().ctx, a = this.X(x1), b = this.X(x2), yy = this.Y(y), i, n, d = 8;
      ctx.strokeStyle = "#333"; ctx.lineWidth = 2.2;
      ctx.beginPath(); ctx.moveTo(a, yy); ctx.lineTo(b, yy); ctx.stroke();
      ctx.lineWidth = 1; n = Math.max(4, Math.round((b - a) / 11));
      for (i = 0; i <= n; i++) { var px = a + i * (b - a) / n; ctx.beginPath(); ctx.moveTo(px, yy); ctx.lineTo(px - d, yy + d); ctx.stroke(); }
    },
    pinned: function (x, y) {                       // apoyo ARTICULADO: triangulo + rayado
      var ctx = this.S().ctx, px = this.X(x), py = this.Y(y), w = 8, h = 11, i;
      ctx.strokeStyle = "#333"; ctx.fillStyle = "#fff"; ctx.lineWidth = 1.6;
      ctx.beginPath(); ctx.moveTo(px, py); ctx.lineTo(px - w, py + h); ctx.lineTo(px + w, py + h); ctx.closePath(); ctx.stroke();
      ctx.lineWidth = 1;
      for (i = 0; i <= 5; i++) { var qx = px - w + i * 2 * w / 5; ctx.beginPath(); ctx.moveTo(qx, py + h); ctx.lineTo(qx - 5, py + h + 5); ctx.stroke(); }
    },
    quiver: function (x, y, dx, dy, col) {
      var ctx = this.S().ctx, x1 = this.X(x), y1 = this.Y(y), x2 = this.X(x + dx), y2 = this.Y(y + dy);
      ctx.strokeStyle = col; ctx.fillStyle = col; ctx.lineWidth = 2;
      ctx.beginPath(); ctx.moveTo(x1, y1); ctx.lineTo(x2, y2); ctx.stroke();
      var a = Math.atan2(y2 - y1, x2 - x1), h = 8;
      ctx.beginPath(); ctx.moveTo(x2, y2);
      ctx.lineTo(x2 - h * Math.cos(a - 0.4), y2 - h * Math.sin(a - 0.4));
      ctx.lineTo(x2 - h * Math.cos(a + 0.4), y2 - h * Math.sin(a + 0.4));
      ctx.closePath(); ctx.fill();
    },
    moment: function (x, y, col) {
      var ctx = this.S().ctx, cx = this.X(x), cy = this.Y(y), r = 16, a1 = -2.2, a2 = 2.2;
      ctx.strokeStyle = col; ctx.fillStyle = col; ctx.lineWidth = 2.4;
      ctx.beginPath(); ctx.arc(cx, cy, r, a1, a2, false); ctx.stroke();
      var ex = cx + r * Math.cos(a2), ey = cy + r * Math.sin(a2), tx = -Math.sin(a2), ty = Math.cos(a2), h = 8;
      ctx.beginPath(); ctx.moveTo(ex + h * tx, ey + h * ty);
      ctx.lineTo(ex - 0.5 * h * ty, ey + 0.5 * h * tx); ctx.lineTo(ex + 0.5 * h * ty, ey - 0.5 * h * tx);
      ctx.closePath(); ctx.fill();
    },
    text: function (x, y, s, col) {
      var ctx = this.S().ctx; ctx.fillStyle = col || "#333"; ctx.font = "12px Segoe UI";
      ctx.textAlign = "left"; ctx.textBaseline = "middle"; ctx.fillText(s, this.X(x), this.Y(y));
    },
    title: function (s) {
      var st = this.S(), ctx = st.ctx; ctx.fillStyle = "#222"; ctx.font = "bold 14px Segoe UI";
      ctx.textAlign = "center"; ctx.textBaseline = "top"; ctx.fillText(s, st.w / 2, 4); ctx.textAlign = "left";
    },
    xlabel: function (s) {
      var st = this.S(), ctx = st.ctx; ctx.fillStyle = "#333"; ctx.font = "12px Segoe UI";
      ctx.textAlign = "center"; ctx.fillText(s, st.w / 2, st.h - 4); ctx.textAlign = "left";
    },
    ylabel: function (s) {
      var st = this.S(), ctx = st.ctx; ctx.save(); ctx.translate(12, st.h / 2); ctx.rotate(-1.5708);
      ctx.fillStyle = "#333"; ctx.font = "12px Segoe UI"; ctx.textAlign = "center"; ctx.fillText(s, 0, 0); ctx.restore();
    },
    colorbar: function (vmin, vmax) {
      var s = this.S();
      if (s.mode === "3d") { s.prims.push({ k: "cbar", a: vmin, b: vmax }); return; }
      this._cbar(s, vmin, vmax);
    },
    _cbar: function (s, vmin, vmax) {
      var ctx = s.ctx, n = 24, x0 = s.w - 56, y1 = s.mT, y0 = s.h - s.mB, w = 18, i, k;
      for (i = 0; i < n; i++) {
        var t = (i + 0.5) / n, ya = y0 - i / n * (y0 - y1), yb = y0 - (i + 1) / n * (y0 - y1);
        ctx.fillStyle = this.jet(t); ctx.fillRect(x0, yb, w, ya - yb + 0.6);
      }
      ctx.strokeStyle = "#444"; ctx.lineWidth = 1; ctx.strokeRect(x0, y1, w, y0 - y1);
      ctx.fillStyle = "#333"; ctx.font = "11px Segoe UI"; ctx.textAlign = "left"; ctx.textBaseline = "middle";
      for (k = 0; k <= 5; k++) {
        var v = vmin + k / 5 * (vmax - vmin), yy = y0 - k / 5 * (y0 - y1);
        ctx.fillText(this._nf(v), x0 + w + 6, yy);
        ctx.beginPath(); ctx.moveTo(x0 + w, yy); ctx.lineTo(x0 + w + 4, yy); ctx.stroke();
      }
    },
    colormap: function (name) { },

    // ===================== CHARTS MATLAB (elemento por elemento; el .cpd hace el #for) =====================
    bar: function (x, y, w, col) {                  // una barra (de 0 a y), centrada en x, ancho w
      var ctx = this.S().ctx, x1 = this.X(x - w / 2), x2 = this.X(x + w / 2), y0 = this.Y(0), y1 = this.Y(y);
      ctx.fillStyle = col || "#1f6feb"; ctx.fillRect(x1, Math.min(y0, y1), x2 - x1, Math.abs(y1 - y0));
      ctx.strokeStyle = "rgba(0,0,0,0.35)"; ctx.lineWidth = 1; ctx.strokeRect(x1, Math.min(y0, y1), x2 - x1, Math.abs(y1 - y0));
    },
    stem: function (x, y, col) {                    // tallo (stem): linea 0->y + circulo
      var ctx = this.S().ctx, c = col || "#1f6feb"; ctx.strokeStyle = c; ctx.lineWidth = 1.5;
      ctx.beginPath(); ctx.moveTo(this.X(x), this.Y(0)); ctx.lineTo(this.X(x), this.Y(y)); ctx.stroke();
      ctx.fillStyle = c; ctx.beginPath(); ctx.arc(this.X(x), this.Y(y), 3.5, 0, 6.2832); ctx.fill();
    },
    area: function (x1, y1, x2, y2, col) {          // relleno hasta y=0 (trapecio) + borde superior
      var ctx = this.S().ctx; ctx.beginPath();
      ctx.moveTo(this.X(x1), this.Y(0)); ctx.lineTo(this.X(x1), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y2)); ctx.lineTo(this.X(x2), this.Y(0)); ctx.closePath();
      ctx.fillStyle = col || "rgba(31,111,235,0.35)"; ctx.fill();
      ctx.strokeStyle = "#1f6feb"; ctx.lineWidth = 1.6; ctx.beginPath(); ctx.moveTo(this.X(x1), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y2)); ctx.stroke();
    },
    stairs: function (x1, y1, x2, y2, col) {        // escalon: horizontal en y1 hasta x2, luego sube a y2
      var ctx = this.S().ctx; ctx.strokeStyle = col || "#333"; ctx.lineWidth = 1.8;
      ctx.beginPath(); ctx.moveTo(this.X(x1), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y1)); ctx.lineTo(this.X(x2), this.Y(y2)); ctx.stroke();
    },
    errorbar: function (x, y, e, col) {             // punto + barra vertical ±e con tapas
      var ctx = this.S().ctx, c = col || "#333", px = this.X(x), pa = this.Y(y - e), pb = this.Y(y + e), w = 4;
      ctx.strokeStyle = c; ctx.lineWidth = 1.4;
      ctx.beginPath(); ctx.moveTo(px, pa); ctx.lineTo(px, pb); ctx.moveTo(px - w, pa); ctx.lineTo(px + w, pa); ctx.moveTo(px - w, pb); ctx.lineTo(px + w, pb); ctx.stroke();
      ctx.fillStyle = c; ctx.beginPath(); ctx.arc(px, this.Y(y), 3, 0, 6.2832); ctx.fill();
    },
    marker: function (x, y, type, col) {            // marcadores MATLAB: o . * + x s d ^ v
      var ctx = this.S().ctx, px = this.X(x), py = this.Y(y), r = 4, c = col || "#1f6feb", t = "" + type;
      ctx.strokeStyle = c; ctx.fillStyle = c; ctx.lineWidth = 1.5;
      if (t === "o") { ctx.beginPath(); ctx.arc(px, py, r, 0, 6.2832); ctx.stroke(); }
      else if (t === ".") { ctx.beginPath(); ctx.arc(px, py, 2, 0, 6.2832); ctx.fill(); }
      else if (t === "*" || t === "+") { ctx.beginPath(); ctx.moveTo(px - r, py); ctx.lineTo(px + r, py); ctx.moveTo(px, py - r); ctx.lineTo(px, py + r); if (t === "*") { ctx.moveTo(px - r * 0.7, py - r * 0.7); ctx.lineTo(px + r * 0.7, py + r * 0.7); ctx.moveTo(px - r * 0.7, py + r * 0.7); ctx.lineTo(px + r * 0.7, py - r * 0.7); } ctx.stroke(); }
      else if (t === "x") { ctx.beginPath(); ctx.moveTo(px - r, py - r); ctx.lineTo(px + r, py + r); ctx.moveTo(px - r, py + r); ctx.lineTo(px + r, py - r); ctx.stroke(); }
      else if (t === "s") { ctx.strokeRect(px - r, py - r, 2 * r, 2 * r); }
      else if (t === "d") { ctx.beginPath(); ctx.moveTo(px, py - r); ctx.lineTo(px + r, py); ctx.lineTo(px, py + r); ctx.lineTo(px - r, py); ctx.closePath(); ctx.stroke(); }
      else if (t === "^") { ctx.beginPath(); ctx.moveTo(px, py - r); ctx.lineTo(px + r, py + r); ctx.lineTo(px - r, py + r); ctx.closePath(); ctx.stroke(); }
      else if (t === "v") { ctx.beginPath(); ctx.moveTo(px, py + r); ctx.lineTo(px + r, py - r); ctx.lineTo(px - r, py - r); ctx.closePath(); ctx.stroke(); }
    },
    imagesc: function (x, y, w, h, t) {             // una celda coloreada (imagesc/pcolor/heatmap) por valor t∈[0,1]
      var ctx = this.S().ctx, x1 = this.X(x), y1 = this.Y(y + h);
      ctx.fillStyle = this.jet(t); ctx.fillRect(x1, y1, this.X(x + w) - x1 + 0.6, this.Y(y) - y1 + 0.6);
    },
    contourCell: function (x0, y0, x1, y1, v00, v10, v11, v01, lv, col) {  // marching squares de 1 celda
      var c = [v00, v10, v11, v01], xx = [x0, x1, x1, x0], yy = [y0, y0, y1, y1], pts = [], i, j;
      for (i = 0; i < 4; i++) { j = (i + 1) % 4; var a = c[i], b = c[j]; if ((a < lv) !== (b < lv)) { var tt = (lv - a) / (b - a); pts.push(xx[i] + tt * (xx[j] - xx[i]), yy[i] + tt * (yy[j] - yy[i])); } }
      if (pts.length >= 4) { var ctx = this.S().ctx; ctx.strokeStyle = col || "#222"; ctx.lineWidth = 1.2; ctx.beginPath(); ctx.moveTo(this.X(pts[0]), this.Y(pts[1])); for (var k = 2; k < pts.length; k += 2) ctx.lineTo(this.X(pts[k]), this.Y(pts[k + 1])); ctx.stroke(); }
    },
    polar: function (theta, r, col) {               // un punto polar (theta en GRADOS); usar axis simetrico ±rmax
      var a = theta * 0.0174533, ctx = this.S().ctx;
      ctx.fillStyle = col || "#1f6feb"; ctx.beginPath(); ctx.arc(this.X(r * Math.cos(a)), this.Y(r * Math.sin(a)), 3, 0, 6.2832); ctx.fill();
    },
    legend: function (x, y, label, col) {           // una entrada de leyenda (swatch + texto) en coords de DATOS
      var ctx = this.S().ctx, px = this.X(x), py = this.Y(y);
      ctx.fillStyle = col || "#1f6feb"; ctx.fillRect(px, py - 5, 16, 3.5);
      ctx.fillStyle = "#222"; ctx.font = "11px Segoe UI"; ctx.textAlign = "left"; ctx.textBaseline = "middle"; ctx.fillText(label, px + 22, py - 3);
    },
    pie: function (cx, cy, r, a0, a1, col) {         // una porcion de pastel (angulos en GRADOS, a0>a1 = horario); el .cpd recorre las rebanadas
      var s = this.S(), ctx = s.ctx, pcx = this.X(cx), pcy = this.Y(cy), pr = r * s.scx;
      ctx.beginPath(); ctx.moveTo(pcx, pcy); ctx.arc(pcx, pcy, pr, -a0 * 0.0174533, -a1 * 0.0174533, false); ctx.closePath();
      ctx.fillStyle = col || "#1f6feb"; ctx.fill(); ctx.strokeStyle = "#fff"; ctx.lineWidth = 1.5; ctx.stroke();
    },
    compass: function (dx, dy, col) {               // flecha desde el ORIGEN (0,0) — campo polar/direccional
      this.quiver(0, 0, dx, dy, col || "#1f6feb");
    },

    // ===================== 3D (interactivo: arrastrar para orbitar) =====================
    view3: function (azd, eld) { var s = this.S(); s.az = azd * 0.0174533; s.el = eld * 0.0174533; },
    axis3: function (x0, x1, y0, y1, z0, z1) {
      var s = this.S(); s.mode = "3d"; s.prims = []; s.bb = [x0, x1, y0, y1, z0, z1];
    },
    fill3: function (p, t) { this.S().prims.push({ k: "face", p: p, c: this.jet(t) }); },
    line3: function (x1, y1, z1, x2, y2, z2, col) { this.S().prims.push({ k: "line3", p: [x1, y1, z1, x2, y2, z2], c: col }); },
    text3: function (x, y, z, str, col) { this.S().prims.push({ k: "text3", p: [x, y, z], s: str, c: col || "#333" }); },
    _pr: function (s, x, y, z) {                       // proyeccion ortografica con s.az, s.el
      var ca = Math.cos(s.az), sa = Math.sin(s.az), ce = Math.cos(s.el), se = Math.sin(s.el);
      var x1 = x * ca + y * sa, y1 = -x * sa + y * ca;
      return [x1, -(y1 * se) + z * ce, y1 * ce + z * se];
    },
    _fit: function (s) {                               // escala/centro segun bbox proyectada
      var b = s.bb, p = [], i, j, k;
      for (i = 0; i < 2; i++) for (j = 0; j < 2; j++) for (k = 0; k < 2; k++)
        p.push(this._pr(s, i ? b[1] : b[0], j ? b[3] : b[2], k ? b[5] : b[4]));
      var mnx = 1e9, mxx = -1e9, mny = 1e9, mxy = -1e9;
      for (i = 0; i < p.length; i++) { mnx = Math.min(mnx, p[i][0]); mxx = Math.max(mxx, p[i][0]); mny = Math.min(mny, p[i][1]); mxy = Math.max(mxy, p[i][1]); }
      var aW = s.w - 2 * s.padx - 80, aH = s.h - 2 * s.pady;
      s.sc3 = Math.min(aW / (mxx - mnx), aH / (mxy - mny));
      s.ox3 = s.padx + (aW - (mxx - mnx) * s.sc3) / 2 - mnx * s.sc3;
      s.oy3 = s.h - s.pady - (aH - (mxy - mny) * s.sc3) / 2 + mny * s.sc3;
    },
    _P: function (s, x, y, z) { var q = this._pr(s, x, y, z); return [s.ox3 + q[0] * s.sc3, s.oy3 - q[1] * s.sc3, q[2]]; },
    _draw3: function (s) {                             // re-dibuja toda la escena 3D
      var ctx = s.ctx, ML = this, i, k, q;
      ctx.clearRect(0, 0, s.w, s.h);
      this._fit(s);
      var items = [];
      for (i = 0; i < s.prims.length; i++) {
        var o = s.prims[i];
        if (o.k === "face") {
          var poly = [], d = 0, n = o.p.length / 3;
          for (k = 0; k < n; k++) { q = ML._P(s, o.p[3 * k], o.p[3 * k + 1], o.p[3 * k + 2]); poly.push(q[0], q[1]); d += q[2]; }
          items.push({ k: "face", poly: poly, depth: d / n, c: o.c });
        } else if (o.k === "line3") {
          var a = ML._P(s, o.p[0], o.p[1], o.p[2]), b = ML._P(s, o.p[3], o.p[4], o.p[5]);
          items.push({ k: "line", a: a, b: b, depth: (a[2] + b[2]) / 2, c: o.c });
        } else if (o.k === "text3") {
          q = ML._P(s, o.p[0], o.p[1], o.p[2]); items.push({ k: "text", x: q[0], y: q[1], s: o.s, depth: q[2] + 1e6, c: o.c });
        }
      }
      items.sort(function (a, b) { return a.depth - b.depth; });
      for (i = 0; i < items.length; i++) {
        var t = items[i];
        if (t.k === "face") {
          ctx.beginPath(); ctx.moveTo(t.poly[0], t.poly[1]);
          for (k = 2; k < t.poly.length; k += 2) ctx.lineTo(t.poly[k], t.poly[k + 1]);
          ctx.closePath(); ctx.fillStyle = t.c; ctx.fill();
          ctx.strokeStyle = "rgba(0,0,0,0.22)"; ctx.lineWidth = 0.4; ctx.stroke();
        } else if (t.k === "line") {
          ctx.strokeStyle = t.c; ctx.lineWidth = 2.2; ctx.beginPath(); ctx.moveTo(t.a[0], t.a[1]); ctx.lineTo(t.b[0], t.b[1]); ctx.stroke();
        } else { ctx.fillStyle = t.c; ctx.font = "12px Segoe UI"; ctx.textAlign = "left"; ctx.textBaseline = "middle"; ctx.fillText(t.s, t.x + 4, t.y); }
      }
      for (i = 0; i < s.prims.length; i++) if (s.prims[i].k === "cbar") this._cbar(s, s.prims[i].a, s.prims[i].b);
      ctx.fillStyle = "#999"; ctx.font = "10px Segoe UI"; ctx.textAlign = "left"; ctx.textBaseline = "bottom";
      ctx.fillText("(arrastra para girar)", 6, s.h - 4);
    },
    render3: function () {                             // primer dibujo + handlers de rotacion
      var ML = this, s = this.S(), cv = s.cv;
      ML._draw3(s);
      cv.style.cursor = "grab";
      var drag = false, x0 = 0, y0 = 0, az0 = 0, el0 = 0;
      cv.onmousedown = function (e) { drag = true; x0 = e.clientX; y0 = e.clientY; az0 = s.az; el0 = s.el; cv.style.cursor = "grabbing"; e.preventDefault(); };
      cv.onmousemove = function (e) {
        if (!drag) return;
        s.az = az0 + (e.clientX - x0) * 0.01;
        s.el = el0 + (e.clientY - y0) * 0.01;
        if (s.el > 1.5) s.el = 1.5; if (s.el < -1.5) s.el = -1.5;
        ML._draw3(s);
      };
      var up = function () { drag = false; cv.style.cursor = "grab"; };
      cv.onmouseup = up; cv.onmouseleave = up;
    },

    // ===================== STEPPER (botones "paso a paso") =====================
    // steps$("titulo") -> step$("titulo paso"; "texto") x N -> endsteps$
    // El usuario navega con los botones ◀ Anterior / Siguiente ▶. En el texto:
    //   *negrita*  y  //  = salto de linea. (sin " ; ' < > para no romper el .cpd)
    steps: function (title) {
      var wrap = document.createElement("div");
      wrap.style.cssText = "display:inline-block;vertical-align:top;width:560px;max-width:100%;margin:6px;border:1px solid #cfd6dd;border-radius:10px;background:#fff;font:13px Segoe UI;box-shadow:0 1px 4px rgba(0,0,0,0.08);overflow:hidden";
      var head = document.createElement("div");
      head.style.cssText = "background:#1f6feb;color:#fff;padding:8px 14px;font-weight:bold;font-size:14px";
      head.textContent = title || "Pasos";
      var body = document.createElement("div");
      body.style.cssText = "padding:14px 16px;min-height:96px;line-height:1.55;color:#222";
      var foot = document.createElement("div");
      foot.style.cssText = "display:flex;align-items:center;gap:10px;padding:8px 14px;border-top:1px solid #eee;background:#fafbfc";
      var bcss = "border:1px solid #1f6feb;border-radius:6px;padding:5px 14px;font:13px Segoe UI;cursor:pointer";
      var bprev = document.createElement("button"); bprev.style.cssText = bcss + ";background:#fff;color:#1f6feb"; bprev.textContent = "◀ Anterior";
      var bnext = document.createElement("button"); bnext.style.cssText = bcss + ";background:#1f6feb;color:#fff"; bnext.textContent = "Siguiente ▶";
      var dots = document.createElement("div"); dots.style.cssText = "flex:1;text-align:center;color:#9aa4ad;letter-spacing:2px";
      foot.appendChild(bprev); foot.appendChild(dots); foot.appendChild(bnext);
      wrap.appendChild(head); wrap.appendChild(body); wrap.appendChild(foot);
      var sc = document.currentScript;
      if (sc && sc.parentNode) sc.parentNode.insertBefore(wrap, sc); else document.body.appendChild(wrap);
      this._stp = { cards: [], cur: 0, body: body, dots: dots, prev: bprev, next: bnext };
    },
    step: function (t, html) { if (this._stp) this._stp.cards.push({ t: t, h: html }); },
    endsteps: function () {
      var st = this._stp; if (!st || !st.cards.length) return;
      function fmt(s) { return String(s).replace(/\*([^*]+)\*/g, "<b>$1</b>").replace(/\/\//g, "<br>"); }
      function show(i) {
        st.cur = Math.max(0, Math.min(st.cards.length - 1, i));
        var c = st.cards[st.cur];
        st.body.innerHTML = "<div style='font-weight:bold;color:#1f6feb;margin-bottom:7px;font-size:14px'>" + fmt(c.t) + "</div>" + fmt(c.h);
        var d = ""; for (var k = 0; k < st.cards.length; k++) d += (k === st.cur ? "●" : "○") + " ";
        st.dots.innerHTML = d + "<span style='margin-left:6px;font-size:12px;letter-spacing:0'>" + (st.cur + 1) + " / " + st.cards.length + "</span>";
        st.prev.style.opacity = st.cur === 0 ? 0.4 : 1;
        st.next.style.opacity = st.cur === st.cards.length - 1 ? 0.4 : 1;
      }
      st.prev.onclick = function () { show(st.cur - 1); };
      st.next.onclick = function () { show(st.cur + 1); };
      show(0);
      this._stp = null;
    }
  };
  global.ML = ML;
})(window);
"""";

        public const string MlplotCpd = """"
#hide
"mlplot.cpd — API de graficas estilo MATLAB para Calcpad.
"Todo el dibujo (canvas/<>) vive en mlplot.js. Estas #def SOLO llaman a ML.* :
"ningun .cpd del usuario escribe '<> , solo: #include mlplot.cpd ; calcular ; graficar.
"Nombres = funciones de MATLAB. Color = ULTIMO argumento, pelado (red, #0000cc, royalblue).
"
"USO:
"  #include mlplot.cpd
"  ... calculos ...
"  #val
"  figure$(fig1; W; H)        ' abre el lienzo (w,h en px)
"  axis$(x0; x1; y0; y1)      ' rango de datos -> mapea a pixeles
"  patch$(x1;y1; x2;y2; x3;y3; x4;y4; t)   ' cuadrilatero coloreado por t (0..1), jet_r
"  plot$(x1;y1; x2;y2; col)   ' segmento
"  quiver$(x;y; dx;dy; col)   ' flecha (vector)
"  moment$(x;y; col)          ' flecha curva de momento
"  text$(x;y; s; col) | title$(s) | xlabel$(s) | ylabel$(s)
"  colorbar$(vmin; vmax)      ' barra de color con valores
"  endfig$                    ' cierra
"  #equ
"
"--- ENVOLTORIO (unica parte con <> ; carga mlplot.js y abre el <script>) ---
#def figure$(id$; w$; h$) = '<script src="https://calcpad.local/mlplot.js"></script><script>ML.figure("id$", 'w$', 'h$');
#def endfig$ = '</script>
"--- EJES ---
#def axis$(x0$; x1$; y0$; y1$) = 'ML.axis('x0$', 'x1$', 'y0$', 'y1$');
#def axischart$(x0$; x1$; y0$; y1$) = 'ML.axischart('x0$', 'x1$', 'y0$', 'y1$');
#def semilogx$(x0$; x1$; y0$; y1$) = 'ML.axislog('x0$', 'x1$', 'y0$', 'y1$', 1, 0);
#def semilogy$(x0$; x1$; y0$; y1$) = 'ML.axislog('x0$', 'x1$', 'y0$', 'y1$', 0, 1);
#def loglog$(x0$; x1$; y0$; y1$) = 'ML.axislog('x0$', 'x1$', 'y0$', 'y1$', 1, 1);
#def grid$(ndx$; ndy$) = 'ML.grid('ndx$', 'ndy$');
#def datatip$(s$) = 'ML.datatip("s$");
#def datapoint$(x$; y$; v$) = 'ML.datapoint('x$', 'y$', 'v$');
#def hold$ = 'ML.hold();
"--- RELLENOS / MALLAS ---
#def patch$(x1$; y1$; x2$; y2$; x3$; y3$; x4$; y4$; t$) = 'ML.patch(['x1$', 'y1$', 'x2$', 'y2$', 'x3$', 'y3$', 'x4$', 'y4$'], 't$');
#def fill$(x1$; y1$; x2$; y2$; x3$; y3$; x4$; y4$; col$) = 'ML.fill(['x1$', 'y1$', 'x2$', 'y2$', 'x3$', 'y3$', 'x4$', 'y4$'], "col$");
#def rectangle$(x1$; y1$; x2$; y2$; col$) = 'ML.rectangle('x1$', 'y1$', 'x2$', 'y2$', "col$");
"--- LINEAS / PUNTOS ---
#def plot$(x1$; y1$; x2$; y2$; col$) = 'ML.plot('x1$', 'y1$', 'x2$', 'y2$', "col$");
#def line$(x1$; y1$; x2$; y2$; col$) = 'ML.line('x1$', 'y1$', 'x2$', 'y2$', "col$");
#def scatter$(x$; y$; col$) = 'ML.scatter('x$', 'y$', "col$");
#def fixed$(x1$; y$; x2$) = 'ML.fixed('x1$', 'y$', 'x2$');
#def pinned$(x$; y$) = 'ML.pinned('x$', 'y$');
"--- CHARTS MATLAB (1 elemento por llamada ; el .cpd hace el #for) ---
#def bar$(x$; y$; w$; col$) = 'ML.bar('x$', 'y$', 'w$', "col$");
#def stem$(x$; y$; col$) = 'ML.stem('x$', 'y$', "col$");
#def area$(x1$; y1$; x2$; y2$; col$) = 'ML.area('x1$', 'y1$', 'x2$', 'y2$', "col$");
#def stairs$(x1$; y1$; x2$; y2$; col$) = 'ML.stairs('x1$', 'y1$', 'x2$', 'y2$', "col$");
#def errorbar$(x$; y$; e$; col$) = 'ML.errorbar('x$', 'y$', 'e$', "col$");
#def marker$(x$; y$; t$; col$) = 'ML.marker('x$', 'y$', "t$", "col$");
#def imagesc$(x$; y$; w$; h$; t$) = 'ML.imagesc('x$', 'y$', 'w$', 'h$', 't$');
#def contour$(x0$; y0$; x1$; y1$; v00$; v10$; v11$; v01$; lv$; col$) = 'ML.contourCell('x0$', 'y0$', 'x1$', 'y1$', 'v00$', 'v10$', 'v11$', 'v01$', 'lv$', "col$");
#def polar$(th$; r$; col$) = 'ML.polar('th$', 'r$', "col$");
#def legend$(x$; y$; s$; col$) = 'ML.legend('x$', 'y$', "s$", "col$");
#def pie$(cx$; cy$; r$; a0$; a1$; col$) = 'ML.pie('cx$', 'cy$', 'r$', 'a0$', 'a1$', "col$");
#def compass$(dx$; dy$; col$) = 'ML.compass('dx$', 'dy$', "col$");
"--- FLECHAS ---
#def quiver$(x$; y$; dx$; dy$; col$) = 'ML.quiver('x$', 'y$', 'dx$', 'dy$', "col$");
#def moment$(x$; y$; col$) = 'ML.moment('x$', 'y$', "col$");
"--- TEXTO / TITULOS ---
#def text$(x$; y$; s$; col$) = 'ML.text('x$', 'y$', "s$", "col$");
#def title$(s$) = 'ML.title("s$");
#def xlabel$(s$) = 'ML.xlabel("s$");
#def ylabel$(s$) = 'ML.ylabel("s$");
"--- COLOR ---
#def colorbar$(vmin$; vmax$) = 'ML.colorbar('vmin$', 'vmax$');
#def colormap$(name$) = 'ML.colormap("name$");
"--- 3D (surf / mesh / fill3) ; flujo: figure$ view3$ axis3$ fill3$... render3$ colorbar$ endfig$ ---
#def figure3$(id$; w$; h$) = '<script src="https://calcpad.local/glplot.js"></script><script>GL3.figure3("id$", 'w$', 'h$');
#def line3$(x1$; y1$; z1$; x2$; y2$; z2$; col$) = 'GL3.line3('x1$', 'y1$', 'z1$', 'x2$', 'y2$', 'z2$', "col$");
#def tick3$(x$; y$; z$; v$) = 'GL3.tick3('x$', 'y$', 'z$', 'v$');
#def label3$(x$; y$; z$; s$) = 'GL3.tick3('x$', 'y$', 'z$', "s$");
#def cartesian3$(x0$; x1$; y0$; y1$; z0$; z1$; ndx$; ndz$) = 'GL3.cartesian3('x0$', 'x1$', 'y0$', 'y1$', 'z0$', 'z1$', 'ndx$', 'ndz$');
#def datatip3$(s$) = 'GL3.datatip("s$");
#def datapoint3$(x$; z$; v$) = 'GL3.datapoint('x$', 0, 'z$', 'v$');
#def datapoint3d$(x$; y$; z$; v$) = 'GL3.datapoint('x$', 'y$', 'z$', 'v$');
#def colorbar3$(vmin$; vmax$; h$) = '<script>GL3.colorbar3('vmin$', 'vmax$', 'h$');</script>
#def view3$(az$; el$) = 'GL3.view3('az$', 'el$');
#def axis3$(x0$; x1$; y0$; y1$; z0$; z1$) = 'GL3.axis3('x0$', 'x1$', 'y0$', 'y1$', 'z0$', 'z1$');
#def fill3$(x1$; y1$; z1$; x2$; y2$; z2$; x3$; y3$; z3$; x4$; y4$; z4$; t1$; t2$; t3$; t4$) = 'GL3.fill3(['x1$', 'y1$', 'z1$', 'x2$', 'y2$', 'z2$', 'x3$', 'y3$', 'z3$', 'x4$', 'y4$', 'z4$'], 't1$', 't2$', 't3$', 't4$');
"--- 3D charts (scatter3 / quiver3 / stem3) ---
#def scatter3$(x$; y$; z$; t$) = 'GL3.point3('x$', 'y$', 'z$', 't$');
#def quiver3$(x$; y$; z$; dx$; dy$; dz$; col$) = 'GL3.quiver3('x$', 'y$', 'z$', 'dx$', 'dy$', 'dz$', "col$");
#def stem3$(x$; y$; z$; t$) = 'GL3.stem3('x$', 'y$', 'z$', 't$');
#def trisurf$(x1$; y1$; z1$; x2$; y2$; z2$; x3$; y3$; z3$; t1$; t2$; t3$) = 'GL3.tri3('x1$', 'y1$', 'z1$', 'x2$', 'y2$', 'z2$', 'x3$', 'y3$', 'z3$', 't1$', 't2$', 't3$');
#def sphere3$(cx$; cy$; cz$; r$; t$) = 'GL3.sphere('cx$', 'cy$', 'cz$', 'r$', 't$');
#def cylinder3$(cx$; cy$; z0$; z1$; r$; t$) = 'GL3.cylinder('cx$', 'cy$', 'z0$', 'z1$', 'r$', 't$');
#def lighting$(on$) = 'GL3.lighting("on$");
#def shading$(m$) = 'GL3.shading("m$");
#def render3$ = 'GL3.render3();
"--- BOTONES paso a paso (stepper) ; flujo: steps$ step$... endsteps$ ---
"  En el texto de step$: *negrita* y // = salto de linea. Evitar " ; ' < > en el texto.
#def steps$(titulo$) = '<script src="https://calcpad.local/mlplot.js"></script><script>ML.steps("titulo$");
#def step$(t$; html$) = 'ML.step("t$", "html$");
#def endsteps$ = 'ML.endsteps();</script>
#show
"""";

    }
}