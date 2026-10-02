(function () {
    function readJson(id) {
        var element = document.getElementById(id);
        if (!element) return [];
        try { return JSON.parse(element.textContent || "[]"); }
        catch { return []; }
    }

    function css(name) {
        return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
    }

    function drawLineChart(canvas, points) {
        if (!canvas) return;
        var ctx = canvas.getContext("2d");
        var w = canvas.width;
        var h = canvas.height;
        var pad = 42;
        var values = points.map(function (p) { return Number(p.revenue || p.Revenue || 0); });
        var labels = points.map(function (p) { return p.month || p.Month; });
        var max = Math.max.apply(null, values.concat([1000]));
        ctx.clearRect(0, 0, w, h);

        var gradient = ctx.createLinearGradient(0, pad, 0, h - pad);
        gradient.addColorStop(0, "rgba(37, 99, 235, .32)");
        gradient.addColorStop(1, "rgba(37, 99, 235, 0)");

        ctx.strokeStyle = "#dbe3ef";
        ctx.lineWidth = 1;
        ctx.font = "13px Arial";
        ctx.fillStyle = "#64748b";
        for (var i = 0; i < 4; i++) {
            var y = pad + ((h - pad * 2) / 3) * i;
            ctx.beginPath();
            ctx.moveTo(pad, y);
            ctx.lineTo(w - pad, y);
            ctx.stroke();
        }

        var coords = values.map(function (value, index) {
            var x = pad + ((w - pad * 2) / Math.max(1, values.length - 1)) * index;
            var y = h - pad - ((h - pad * 2) * value / max);
            return { x: x, y: y };
        });

        ctx.beginPath();
        coords.forEach(function (point, index) {
            if (index === 0) ctx.moveTo(point.x, h - pad);
            ctx.lineTo(point.x, point.y);
        });
        if (coords.length) {
            ctx.lineTo(coords[coords.length - 1].x, h - pad);
            ctx.closePath();
            ctx.fillStyle = gradient;
            ctx.fill();
        }

        ctx.beginPath();
        coords.forEach(function (point, index) {
            if (index === 0) ctx.moveTo(point.x, point.y);
            else ctx.lineTo(point.x, point.y);
        });
        ctx.strokeStyle = "#2563eb";
        ctx.lineWidth = 4;
        ctx.lineJoin = "round";
        ctx.stroke();

        coords.forEach(function (point, index) {
            ctx.beginPath();
            ctx.arc(point.x, point.y, 5, 0, Math.PI * 2);
            ctx.fillStyle = "#ffffff";
            ctx.fill();
            ctx.strokeStyle = "#2563eb";
            ctx.lineWidth = 3;
            ctx.stroke();
            ctx.fillStyle = "#64748b";
            ctx.fillText(labels[index] || "", point.x - 26, h - 13);
        });
    }

    function drawDonutChart(canvas, points) {
        if (!canvas) return;
        var ctx = canvas.getContext("2d");
        var w = canvas.width;
        var h = canvas.height;
        var cx = w / 2;
        var cy = h / 2 - 8;
        var radius = Math.min(w, h) / 2 - 38;
        var total = points.reduce(function (sum, p) { return sum + Number(p.count || p.Count || 0); }, 0);
        var colors = ["#2563eb", "#f59e0b", "#16a34a", "#dc2626", "#64748b"];
        ctx.clearRect(0, 0, w, h);

        if (total === 0) {
            points = points.map(function (p) { return { status: p.status || p.Status, count: 1 }; });
            total = points.length || 1;
        }

        var start = -Math.PI / 2;
        points.forEach(function (point, index) {
            var value = Number(point.count || point.Count || 0);
            var slice = (value / total) * Math.PI * 2;
            ctx.beginPath();
            ctx.moveTo(cx, cy);
            ctx.arc(cx, cy, radius, start, start + slice);
            ctx.closePath();
            ctx.fillStyle = colors[index % colors.length];
            ctx.fill();
            start += slice;
        });

        ctx.beginPath();
        ctx.arc(cx, cy, radius * .62, 0, Math.PI * 2);
        ctx.fillStyle = "#ffffff";
        ctx.fill();
        ctx.fillStyle = "#0f172a";
        ctx.font = "700 26px Arial";
        ctx.textAlign = "center";
        ctx.fillText(String(points.reduce(function (sum, p) { return sum + Number(p.count || p.Count || 0); }, 0)), cx, cy + 5);
        ctx.font = "13px Arial";
        ctx.fillStyle = "#64748b";
        ctx.fillText("orders", cx, cy + 27);
        ctx.textAlign = "start";
    }

    document.addEventListener("DOMContentLoaded", function () {
        drawLineChart(document.getElementById("salesChart"), readJson("salesChartData"));
        drawDonutChart(document.getElementById("statusChart"), readJson("statusChartData"));
    });
})();
