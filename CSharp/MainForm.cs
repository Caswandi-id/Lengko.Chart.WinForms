using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Lengko.Chart.WinForms;

namespace Lengko.Chart.Demo.CSharp;

public class MainForm : Form
{
    private bool _isDark = false;
    private readonly Random _rand = new();
    private readonly List<(Panel Card, Label Title, Label Subtitle, Control Chart)> _cards = new();

    // Chart references for toolbar actions
    private LengkoBarChart _barChart = null!;
    private LengkoHorizontalBarChart _hBarChart = null!;
    private LengkoLineChart _lineChart = null!;
    private LengkoAreaChart _areaChart = null!;
    private LengkoPieChart _pieChart = null!;
    private LengkoDonutChart _donutChart = null!;
    private LengkoRadarChart _radarChart = null!;
    private LengkoRadialGauge _gaugeChart = null!;
    private LengkoBulletGraph _bulletChart = null!;
    private LengkoSparkline _sparklineChart = null!;

    public MainForm()
    {
        InitializeUi();
        PopulateAllCharts();
    }

    private void InitializeUi()
    {
        Text = "Lengko.Chart.WinForms Showcase (All 32 Charts)";
        Width = 1240;
        Height = 880;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.FromArgb(248, 250, 252);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(16)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56)); // Header
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); // Toolbar
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Main Tabs
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28)); // Footer

        // 1. Header
        var header = new Panel { Dock = DockStyle.Fill };
        var title = new Label
        {
            Text = "Lengko.Chart.WinForms Showcase (All 32 Charts)",
            Font = new Font("Segoe UI", 15f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = new Point(0, 0)
        };
        var subTitle = new Label
        {
            Text = "Complete catalog: 32 WinForms chart controls with pure GDI+ rendering, hover tooltips, and themes.",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = new Point(0, 28)
        };
        header.Controls.Add(title);
        header.Controls.Add(subTitle);
        root.Controls.Add(header, 0, 0);

        // 2. Toolbar
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0)
        };

        var btnTheme = CreateToolbarButton("Theme: Light", Color.FromArgb(71, 85, 105));
        var btnDonut = CreateToolbarButton("Pie Mode: Standard", Color.FromArgb(2, 132, 199));
        var btnSmooth = CreateToolbarButton("Line Curve: Smooth", Color.FromArgb(5, 150, 105));
        var btnRandomize = CreateToolbarButton("Randomize Data", Color.FromArgb(139, 92, 246));

        toolbar.Controls.Add(btnTheme);
        toolbar.Controls.Add(btnDonut);
        toolbar.Controls.Add(btnSmooth);
        toolbar.Controls.Add(btnRandomize);
        root.Controls.Add(toolbar, 0, 1);

        // 3. TabControl with 6 Categories
        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Padding = new Point(14, 8)
        };

        // Tab 1: Bars & Columns (6)
        var tabBars = new TabPage("Bars & Columns (6)");
        var gridBars = CreateGrid(3);
        gridBars.Controls.Add(CreateCard("Bar Chart (LengkoBarChart)", "Multi-series grouped column comparison", _barChart = new LengkoBarChart { ShowGrid = true, ShowValues = true }), 0, 0);
        gridBars.Controls.Add(CreateCard("Horizontal Bar Chart (LengkoHorizontalBarChart)", "Ranked category comparison with horizontal bars", _hBarChart = new LengkoHorizontalBarChart { ShowValues = true }), 1, 0);
        var stackedBar = new LengkoStackedBarChart { ShowGrid = true };
        gridBars.Controls.Add(CreateCard("Stacked Bar Chart (LengkoStackedBarChart)", "Part-to-whole segment distribution across periods", stackedBar), 0, 1);
        var hist = new LengkoHistogramChart { ShowGrid = true };
        gridBars.Controls.Add(CreateCard("Histogram Chart (LengkoHistogramChart)", "Frequency distribution across numerical range bins", hist), 1, 1);
        var pareto = new LengkoParetoChart();
        gridBars.Controls.Add(CreateCard("Pareto Chart (LengkoParetoChart)", "80/20 defect / factor analysis with cumulative line", pareto), 0, 2);
        var waterfall = new LengkoWaterfallChart();
        gridBars.Controls.Add(CreateCard("Waterfall Chart (LengkoWaterfallChart)", "Incremental revenue, expense, and net variance flow", waterfall), 1, 2);
        tabBars.Controls.Add(gridBars);
        tabs.TabPages.Add(tabBars);

        // Tab 2: Lines & Areas (6)
        var tabLines = new TabPage("Lines & Areas (6)");
        var gridLines = CreateGrid(3);
        gridLines.Controls.Add(CreateCard("Line Chart (LengkoLineChart)", "Multi-series continuous trends with points and fill", _lineChart = new LengkoLineChart { SmoothCurves = true, ShowPoints = true, FillArea = true, ShowGrid = true }), 0, 0);
        gridLines.Controls.Add(CreateCard("Area Chart (LengkoAreaChart)", "Cumulative trend volume with smooth area fill", _areaChart = new LengkoAreaChart { SmoothCurves = true, ShowGrid = true }), 1, 0);
        var stackedArea = new LengkoStackedAreaChart { ShowGrid = true };
        gridLines.Controls.Add(CreateCard("Stacked Area Chart (LengkoStackedAreaChart)", "Cumulative regional contribution over time", stackedArea), 0, 1);
        var stepLine = new LengkoStepLineChart { ShowPoints = true, FillArea = true };
        gridLines.Controls.Add(CreateCard("Step Line Chart (LengkoStepLineChart)", "Discrete step-wise inventory and threshold changes", stepLine), 1, 1);
        var combo = new LengkoComboChart { ShowGrid = true };
        gridLines.Controls.Add(CreateCard("Combo Chart (LengkoComboChart)", "Dual-axis combination of volume bars and profit line", combo), 0, 2);
        var generic = new LengkoChart { Title = "Multi-Point Metric" };
        gridLines.Controls.Add(CreateCard("Generic Chart (LengkoChart)", "Lightweight multi-point metric visualization", generic), 1, 2);
        tabLines.Controls.Add(gridLines);
        tabs.TabPages.Add(tabLines);

        // Tab 3: Circular & Polar (5)
        var tabCirc = new TabPage("Circular & Polar (5)");
        var gridCirc = CreateGrid(3);
        gridCirc.Controls.Add(CreateCard("Pie Chart (LengkoPieChart)", "Proportional slice distribution with percentages", _pieChart = new LengkoPieChart { ShowPercentages = true }), 0, 0);
        gridCirc.Controls.Add(CreateCard("Donut Chart (LengkoDonutChart)", "Ring distribution with central readout and legend", _donutChart = new LengkoDonutChart { ShowPercentages = true }), 1, 0);
        gridCirc.Controls.Add(CreateCard("Radar Chart (LengkoRadarChart)", "Multi-variable capability spider comparison", _radarChart = new LengkoRadarChart()), 0, 1);
        var polar = new LengkoPolarAreaChart();
        gridCirc.Controls.Add(CreateCard("Polar Area Chart (LengkoPolarAreaChart)", "Cyclic and directional magnitude comparison", polar), 1, 1);
        var sunburst = new LengkoSunburstChart();
        gridCirc.Controls.Add(CreateCard("Sunburst Chart (LengkoSunburstChart)", "Multi-level concentric radial hierarchy visualizer", sunburst), 0, 2);
        tabCirc.Controls.Add(gridCirc);
        tabs.TabPages.Add(tabCirc);

        // Tab 4: Financial & Timelines (3)
        var tabFin = new TabPage("Financial & Timelines (3)");
        var gridFin = CreateGrid(3);
        var candle = new LengkoCandlestickChart { ShowVolume = true };
        gridFin.Controls.Add(CreateCard("Candlestick Chart (LengkoCandlestickChart)", "Asset price action with OHLC candles and volume bars", candle), 0, 0);
        var ohlc = new LengkoOhlcChart();
        gridFin.Controls.Add(CreateCard("OHLC Bar Chart (LengkoOhlcChart)", "Open-High-Low-Close financial trading price bars", ohlc), 1, 0);
        var gantt = new LengkoGanttChart();
        gridFin.Controls.Add(CreateCard("Gantt Chart (LengkoGanttChart)", "Project timeline, milestone scheduling, and progress", gantt), 0, 1);
        tabFin.Controls.Add(gridFin);
        tabs.TabPages.Add(tabFin);

        // Tab 5: Hierarchy & Flow (5)
        var tabFlow = new TabPage("Hierarchy & Flow (5)");
        var gridFlow = CreateGrid(3);
        var treemap = new LengkoTreemap();
        gridFlow.Controls.Add(CreateCard("Treemap (LengkoTreemap)", "Hierarchical space-filling rectangular proportions", treemap), 0, 0);
        var sankey = new LengkoSankeyChart();
        gridFlow.Controls.Add(CreateCard("Sankey Flow Chart (LengkoSankeyChart)", "Directed flow paths, quantities, and node transfers", sankey), 1, 0);
        var funnel = new LengkoFunnelChart();
        gridFlow.Controls.Add(CreateCard("Funnel Chart (LengkoFunnelChart)", "Conversion funnel stages and pipeline drop-off", funnel), 0, 1);
        var pyramid = new LengkoPyramidChart();
        gridFlow.Controls.Add(CreateCard("Pyramid Chart (LengkoPyramidChart)", "Hierarchical tiered organizational structure", pyramid), 1, 1);
        var heatmap = new LengkoHeatmapChart();
        gridFlow.Controls.Add(CreateCard("Heatmap Chart (LengkoHeatmapChart)", "2D intensity matrix grid with color spectrum", heatmap), 0, 2);
        tabFlow.Controls.Add(gridFlow);
        tabs.TabPages.Add(tabFlow);

        // Tab 6: KPIs & Statistical (7)
        var tabKpi = new TabPage("KPIs & Statistical (7)");
        var gridKpi = CreateGrid(4);
        gridKpi.Controls.Add(CreateCard("Radial Gauge (LengkoRadialGauge)", "Circular KPI progress arc indicator with centered metrics", _gaugeChart = new LengkoRadialGauge()), 0, 0);
        gridKpi.Controls.Add(CreateCard("Bullet Graph (LengkoBulletGraph)", "Compact KPI against target and qualitative performance bands", _bulletChart = new LengkoBulletGraph()), 1, 0);
        gridKpi.Controls.Add(CreateCard("Sparkline (LengkoSparkline)", "Ultra-compact micro trend visualizer for dashboards", _sparklineChart = new LengkoSparkline()), 0, 1);
        var scatter = new LengkoScatterChart { ShowTrendline = true };
        gridKpi.Controls.Add(CreateCard("Scatter Chart (LengkoScatterChart)", "Bivariate correlation points with linear regression trendline", scatter), 1, 1);
        var bubble = new LengkoBubbleChart();
        gridKpi.Controls.Add(CreateCard("Bubble Chart (LengkoBubbleChart)", "Three-variable correlation mapping X, Y, and magnitude", bubble), 0, 2);
        var boxplot = new LengkoBoxPlotChart();
        gridKpi.Controls.Add(CreateCard("Box Plot Chart (LengkoBoxPlotChart)", "Five-number summary showing min, quartiles, and extremes", boxplot), 1, 2);
        var errorbar = new LengkoErrorBarChart();
        gridKpi.Controls.Add(CreateCard("Error Bar Chart (LengkoErrorBarChart)", "Mean values with upper and lower measurement uncertainty", errorbar), 0, 3);
        tabKpi.Controls.Add(gridKpi);
        tabs.TabPages.Add(tabKpi);

        root.Controls.Add(tabs, 0, 2);

        // 4. Footer Tip
        var footer = new Label
        {
            Text = "💡 Tips: Hover over chart items for interactive tooltips. Scroll wheel to zoom, drag mouse to pan. Click 'Theme' to toggle dark mode.",
            Font = new Font("Segoe UI", 9.0f),
            ForeColor = Color.FromArgb(100, 116, 139),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        root.Controls.Add(footer, 0, 3);

        Controls.Add(root);

        // Wire toolbar event handlers
        btnTheme.Click += (s, e) =>
        {
            _isDark = !_isDark;
            btnTheme.Text = _isDark ? "Theme: Dark" : "Theme: Light";
            var theme = _isDark ? LengkoChartTheme.Dark : LengkoChartTheme.Light;
            LengkoChartThemeScope.DefaultTheme = theme;

            var bg = _isDark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(248, 250, 252);
            var cardBg = _isDark ? Color.FromArgb(30, 41, 59) : Color.White;
            var textPrimary = _isDark ? Color.FromArgb(241, 245, 249) : Color.FromArgb(15, 23, 42);
            var textSecondary = _isDark ? Color.FromArgb(148, 163, 184) : Color.FromArgb(100, 116, 139);

            BackColor = bg;
            title.ForeColor = textPrimary;
            subTitle.ForeColor = textSecondary;
            footer.ForeColor = textSecondary;

            foreach (var c in _cards)
            {
                c.Card.BackColor = cardBg;
                c.Title.ForeColor = textPrimary;
                c.Subtitle.ForeColor = textSecondary;
                c.Chart.BackColor = cardBg;
                c.Card.Invalidate();
                if (c.Chart is ILengkoChartThemedControl themed)
                    themed.ApplyTheme(theme);
                else
                    c.Chart.Invalidate();
            }
        };

        btnDonut.Click += (s, e) =>
        {
            _pieChart.IsDonut = !_pieChart.IsDonut;
            btnDonut.Text = _pieChart.IsDonut ? "Pie Mode: Donut" : "Pie Mode: Standard";
        };

        btnSmooth.Click += (s, e) =>
        {
            _lineChart.SmoothCurves = !_lineChart.SmoothCurves;
            _areaChart.SmoothCurves = !_areaChart.SmoothCurves;
            btnSmooth.Text = _lineChart.SmoothCurves ? "Line Curve: Smooth" : "Line Curve: Straight";
        };

        btnRandomize.Click += (s, e) =>
        {
            foreach (var srs in _barChart.Series)
                for (int i = 0; i < srs.Values.Count; i++)
                    srs.Values[i] = _rand.Next(30, 260);
            _barChart.Invalidate();

            foreach (var it in _hBarChart.Items)
                it.Value = _rand.Next(40, 100);
            _hBarChart.Invalidate();

            foreach (var srs in _lineChart.Series)
                for (int i = 0; i < srs.Values.Count; i++)
                    srs.Values[i] = _rand.Next(15, 110);
            _lineChart.Invalidate();

            foreach (var slc in _pieChart.Slices)
                slc.Value = _rand.Next(10, 60);
            _pieChart.Invalidate();

            foreach (var slc in _donutChart.Slices)
                slc.Value = _rand.Next(10, 60);
            _donutChart.Invalidate();

            foreach (var srs in _radarChart.Series)
                for (int i = 0; i < srs.Values.Count; i++)
                    srs.Values[i] = _rand.Next(40, 99);
            _radarChart.Invalidate();

            _gaugeChart.Value = _rand.Next(20, 98);
            _bulletChart.Value = _rand.Next(40, 95);

            var sparkData = new double[11];
            for (int i = 0; i < 11; i++) sparkData[i] = _rand.Next(10, 65);
            _sparklineChart.SetData(sparkData);
        };
    }

    private Panel CreateCard(string title, string subtitle, Control chart)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(6),
            Padding = new Padding(14, 12, 14, 12),
            BackColor = Color.White
        };

        card.Paint += (s, e) =>
        {
            using var pen = new Pen(_isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(226, 232, 240), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        var pnlHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 44,
            BackColor = Color.Transparent
        };
        var lblTitle = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 10.0f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = new Point(0, 0)
        };
        var lblSubtitle = new Label
        {
            Text = subtitle,
            Font = new Font("Segoe UI", 8.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = new Point(0, 22)
        };
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);

        chart.Dock = DockStyle.Fill;
        card.Controls.Add(chart);
        card.Controls.Add(pnlHeader);
        pnlHeader.SendToBack();

        _cards.Add((card, lblTitle, lblSubtitle, chart));
        return card;
    }

    private TableLayoutPanel CreateGrid(int rows)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = rows,
            AutoScroll = true,
            Padding = new Padding(4)
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        for (int i = 0; i < rows; i++)
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 320f));
        return grid;
    }

    private Button CreateToolbarButton(string text, Color bg)
    {
        var btn = new Button
        {
            Text = text,
            BackColor = bg,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
            Height = 34,
            AutoSize = true,
            Padding = new Padding(10, 4, 10, 4),
            Margin = new Padding(0, 0, 8, 0),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private void PopulateAllCharts()
    {
        // Tab 1: Bars & Columns
        _barChart.Categories.Add("Q1");
        _barChart.Categories.Add("Q2");
        _barChart.Categories.Add("Q3");
        _barChart.Categories.Add("Q4");
        _barChart.Series.Add(new LengkoBarSeries("Revenue", new double[] { 120, 150, 180, 220 }, Color.FromArgb(59, 130, 246)));
        _barChart.Series.Add(new LengkoBarSeries("Profit", new double[] { 45, 60, 75, 95 }, Color.FromArgb(16, 185, 129)));

        _hBarChart.AddItem("TypeScript", 88, Color.FromArgb(59, 130, 246));
        _hBarChart.AddItem("C# / .NET", 94, Color.FromArgb(139, 92, 246));
        _hBarChart.AddItem("Python", 82, Color.FromArgb(245, 158, 11));
        _hBarChart.AddItem("Go", 75, Color.FromArgb(6, 182, 212));
        _hBarChart.AddItem("Rust", 70, Color.FromArgb(239, 68, 68));

        var stackedBar = (LengkoStackedBarChart)_cards[2].Chart;
        stackedBar.Categories.Add("2021");
        stackedBar.Categories.Add("2022");
        stackedBar.Categories.Add("2023");
        stackedBar.Categories.Add("2024");
        stackedBar.Series.Add(new LengkoBarSeries("Cloud", new double[] { 40, 55, 75, 110 }, Color.FromArgb(59, 130, 246)));
        stackedBar.Series.Add(new LengkoBarSeries("On-Prem", new double[] { 60, 50, 45, 30 }, Color.FromArgb(16, 185, 129)));
        stackedBar.Series.Add(new LengkoBarSeries("Support", new double[] { 20, 25, 30, 35 }, Color.FromArgb(245, 158, 11)));

        var hist = (LengkoHistogramChart)_cards[3].Chart;
        hist.SetData(new double[] { 18, 22, 25, 27, 30, 32, 34, 35, 38, 40, 42, 44, 45, 48, 52, 55, 60, 65, 72 }, 6);

        var pareto = (LengkoParetoChart)_cards[4].Chart;
        pareto.Items.Add(new LengkoParetoItem("Timeout 504", 142));
        pareto.Items.Add(new LengkoParetoItem("Auth 401", 95));
        pareto.Items.Add(new LengkoParetoItem("NullRef", 48));
        pareto.Items.Add(new LengkoParetoItem("DB Lock", 24));
        pareto.Items.Add(new LengkoParetoItem("Bad Format", 12));

        var waterfall = (LengkoWaterfallChart)_cards[5].Chart;
        waterfall.Items.Add(new LengkoWaterfallItem("Start Cash", 120, isTotal: true));
        waterfall.Items.Add(new LengkoWaterfallItem("Sales", 52));
        waterfall.Items.Add(new LengkoWaterfallItem("Subscriptions", 30));
        waterfall.Items.Add(new LengkoWaterfallItem("Salaries", -42));
        waterfall.Items.Add(new LengkoWaterfallItem("Cloud", -18));
        waterfall.Items.Add(new LengkoWaterfallItem("Marketing", -14));
        waterfall.Items.Add(new LengkoWaterfallItem("Net Cash", 0, isTotal: true));

        // Tab 2: Lines & Areas
        _lineChart.Series.Add(new LengkoLineSeries("Traffic (k)", new double[] { 22, 34, 45, 40, 58, 65, 80 }, Color.FromArgb(99, 102, 241)));
        _lineChart.Series.Add(new LengkoLineSeries("Signups", new double[] { 10, 18, 24, 28, 35, 42, 55 }, Color.FromArgb(236, 72, 153)));

        _areaChart.Labels.Add("Jan");
        _areaChart.Labels.Add("Feb");
        _areaChart.Labels.Add("Mar");
        _areaChart.Labels.Add("Apr");
        _areaChart.Labels.Add("May");
        _areaChart.Labels.Add("Jun");
        _areaChart.Series.Add(new LengkoAreaSeries("Inbound", new double[] { 30, 45, 52, 68, 74, 90 }, Color.FromArgb(59, 130, 246)));
        _areaChart.Series.Add(new LengkoAreaSeries("Outbound", new double[] { 18, 25, 32, 40, 48, 62 }, Color.FromArgb(16, 185, 129)));

        var stackedArea = (LengkoStackedAreaChart)_cards[8].Chart;
        stackedArea.Categories.Add("Jan");
        stackedArea.Categories.Add("Feb");
        stackedArea.Categories.Add("Mar");
        stackedArea.Categories.Add("Apr");
        stackedArea.Categories.Add("May");
        stackedArea.Categories.Add("Jun");
        stackedArea.Series.Add(new LengkoAreaSeries("North", new double[] { 25, 30, 38, 45, 52, 60 }, Color.FromArgb(14, 165, 233)));
        stackedArea.Series.Add(new LengkoAreaSeries("South", new double[] { 20, 24, 30, 36, 42, 48 }, Color.FromArgb(168, 85, 247)));
        stackedArea.Series.Add(new LengkoAreaSeries("East", new double[] { 15, 18, 22, 28, 32, 38 }, Color.FromArgb(249, 115, 22)));

        var stepLine = (LengkoStepLineChart)_cards[9].Chart;
        stepLine.Series.Add(new LengkoStepSeries("Inventory", new double[] { 100, 85, 85, 60, 120, 95, 70 }, Color.FromArgb(16, 185, 129)));
        stepLine.Series.Add(new LengkoStepSeries("Threshold", new double[] { 50, 50, 50, 50, 50, 50, 50 }, Color.FromArgb(239, 68, 68)));

        var combo = (LengkoComboChart)_cards[10].Chart;
        combo.Categories.Add("Q1");
        combo.Categories.Add("Q2");
        combo.Categories.Add("Q3");
        combo.Categories.Add("Q4");
        combo.BarSeries.Add(new LengkoBarSeries("Units Sold", new double[] { 320, 450, 580, 710 }, Color.FromArgb(59, 130, 246)));
        combo.LineSeries.Add(new LengkoLineSeries("Margin %", new double[] { 22, 28, 26, 31 }, Color.FromArgb(239, 68, 68)));

        var generic = (LengkoChart)_cards[11].Chart;
        generic.AddPoint("CPU", 48);
        generic.AddPoint("RAM", 72);
        generic.AddPoint("Disk", 60);
        generic.AddPoint("Network", 35);
        generic.AddPoint("GPU", 84);

        // Tab 3: Circular & Polar
        _pieChart.Slices.Add(new LengkoPieSlice("North America", 38, Color.FromArgb(59, 130, 246)));
        _pieChart.Slices.Add(new LengkoPieSlice("Europe", 27, Color.FromArgb(16, 185, 129)));
        _pieChart.Slices.Add(new LengkoPieSlice("Asia Pacific", 23, Color.FromArgb(245, 158, 11)));
        _pieChart.Slices.Add(new LengkoPieSlice("Latin America", 12, Color.FromArgb(239, 68, 68)));

        _donutChart.Slices.Add(new LengkoPieSlice("Enterprise", 45, Color.FromArgb(99, 102, 241)));
        _donutChart.Slices.Add(new LengkoPieSlice("SMB", 30, Color.FromArgb(236, 72, 153)));
        _donutChart.Slices.Add(new LengkoPieSlice("Startup", 15, Color.FromArgb(20, 184, 166)));
        _donutChart.Slices.Add(new LengkoPieSlice("Consumer", 10, Color.FromArgb(245, 158, 11)));

        _radarChart.Axes.Add("Speed");
        _radarChart.Axes.Add("Reliability");
        _radarChart.Axes.Add("Usability");
        _radarChart.Axes.Add("Security");
        _radarChart.Axes.Add("Scalability");
        _radarChart.Series.Add(new LengkoRadarSeries("Lengko UI", new double[] { 92, 95, 88, 90, 94 }, Color.FromArgb(139, 92, 246)));
        _radarChart.Series.Add(new LengkoRadarSeries("Standard", new double[] { 65, 70, 60, 75, 68 }, Color.FromArgb(148, 163, 184)));

        var polar = (LengkoPolarAreaChart)_cards[15].Chart;
        polar.Items.Add(new LengkoPolarItem("North", 45, Color.FromArgb(59, 130, 246)));
        polar.Items.Add(new LengkoPolarItem("NE", 60, Color.FromArgb(99, 102, 241)));
        polar.Items.Add(new LengkoPolarItem("East", 80, Color.FromArgb(139, 92, 246)));
        polar.Items.Add(new LengkoPolarItem("SE", 50, Color.FromArgb(236, 72, 153)));
        polar.Items.Add(new LengkoPolarItem("South", 35, Color.FromArgb(244, 63, 94)));
        polar.Items.Add(new LengkoPolarItem("SW", 55, Color.FromArgb(245, 158, 11)));
        polar.Items.Add(new LengkoPolarItem("West", 75, Color.FromArgb(16, 185, 129)));
        polar.Items.Add(new LengkoPolarItem("NW", 40, Color.FromArgb(20, 184, 166)));

        var sunburst = (LengkoSunburstChart)_cards[16].Chart;
        sunburst.Items.Add(new LengkoSunburstItem("Software", 60, 0, Color.FromArgb(59, 130, 246)));
        sunburst.Items.Add(new LengkoSunburstItem("Hardware", 40, 0, Color.FromArgb(16, 185, 129)));
        sunburst.Items.Add(new LengkoSunburstItem("Desktop", 35, 1, Color.FromArgb(99, 102, 241)));
        sunburst.Items.Add(new LengkoSunburstItem("Mobile", 25, 1, Color.FromArgb(14, 165, 233)));
        sunburst.Items.Add(new LengkoSunburstItem("Servers", 25, 1, Color.FromArgb(34, 197, 94)));
        sunburst.Items.Add(new LengkoSunburstItem("IoT", 15, 1, Color.FromArgb(245, 158, 11)));

        // Tab 4: Financial & Timelines
        var candle = (LengkoCandlestickChart)_cards[17].Chart;
        candle.AddCandle("09:30", 150.2, 153.5, 149.8, 152.8, 142000);
        candle.AddCandle("10:00", 152.8, 154.0, 151.2, 151.6, 98000);
        candle.AddCandle("10:30", 151.6, 155.2, 151.0, 154.5, 175000);
        candle.AddCandle("11:00", 154.5, 156.0, 153.8, 155.8, 110000);
        candle.AddCandle("11:30", 155.8, 156.2, 153.0, 153.4, 130000);

        var ohlc = (LengkoOhlcChart)_cards[18].Chart;
        ohlc.Bars.Add(new LengkoOhlcBar("Mon", 100, 108, 98, 105));
        ohlc.Bars.Add(new LengkoOhlcBar("Tue", 105, 110, 102, 104));
        ohlc.Bars.Add(new LengkoOhlcBar("Wed", 104, 114, 103, 112));
        ohlc.Bars.Add(new LengkoOhlcBar("Thu", 112, 115, 108, 109));
        ohlc.Bars.Add(new LengkoOhlcBar("Fri", 109, 118, 107, 116));

        var gantt = (LengkoGanttChart)_cards[19].Chart;
        var baseDate = DateTime.Today;
        gantt.Tasks.Add(new LengkoGanttItem("Architecture & API", baseDate, baseDate.AddDays(5), 100));
        gantt.Tasks.Add(new LengkoGanttItem("Core Chart Engine", baseDate.AddDays(3), baseDate.AddDays(10), 80));
        gantt.Tasks.Add(new LengkoGanttItem("32 WinForms Controls", baseDate.AddDays(6), baseDate.AddDays(15), 55));
        gantt.Tasks.Add(new LengkoGanttItem("Testing & Packaging", baseDate.AddDays(12), baseDate.AddDays(18), 20));

        // Tab 5: Hierarchy & Flow
        var treemap = (LengkoTreemap)_cards[20].Chart;
        treemap.Items.Add(new LengkoTreemapItem("Finance", 420));
        treemap.Items.Add(new LengkoTreemapItem("Engineering", 350));
        treemap.Items.Add(new LengkoTreemapItem("Marketing", 220));
        treemap.Items.Add(new LengkoTreemapItem("Support", 160));
        treemap.Items.Add(new LengkoTreemapItem("Legal & HR", 90));

        var sankey = (LengkoSankeyChart)_cards[21].Chart;
        sankey.Links.Add(new LengkoSankeyLink("Organic Search", "Landing Page", 140));
        sankey.Links.Add(new LengkoSankeyLink("Paid Ads", "Landing Page", 90));
        sankey.Links.Add(new LengkoSankeyLink("Referral", "Product Specs", 65));
        sankey.Links.Add(new LengkoSankeyLink("Landing Page", "Pricing", 130));
        sankey.Links.Add(new LengkoSankeyLink("Landing Page", "Checkout", 70));
        sankey.Links.Add(new LengkoSankeyLink("Pricing", "Checkout", 100));
        sankey.Links.Add(new LengkoSankeyLink("Product Specs", "Checkout", 45));

        var funnel = (LengkoFunnelChart)_cards[22].Chart;
        funnel.Stages.Add(new LengkoFunnelStage("Website Visits", 12500));
        funnel.Stages.Add(new LengkoFunnelStage("Sign Up", 5200));
        funnel.Stages.Add(new LengkoFunnelStage("Cart Added", 2800));
        funnel.Stages.Add(new LengkoFunnelStage("Checkout Started", 1600));
        funnel.Stages.Add(new LengkoFunnelStage("Purchased", 1100));

        var pyramid = (LengkoPyramidChart)_cards[23].Chart;
        pyramid.Stages.Add(new LengkoPyramidStage("Executives", 8));
        pyramid.Stages.Add(new LengkoPyramidStage("Directors", 24));
        pyramid.Stages.Add(new LengkoPyramidStage("Team Leads", 65));
        pyramid.Stages.Add(new LengkoPyramidStage("Specialists", 180));
        pyramid.Stages.Add(new LengkoPyramidStage("Associates", 340));

        var heatmap = (LengkoHeatmapChart)_cards[24].Chart;
        heatmap.RowLabels.Add("Mon");
        heatmap.RowLabels.Add("Tue");
        heatmap.RowLabels.Add("Wed");
        heatmap.RowLabels.Add("Thu");
        heatmap.RowLabels.Add("Fri");
        heatmap.ColumnLabels.Add("09:00");
        heatmap.ColumnLabels.Add("12:00");
        heatmap.ColumnLabels.Add("15:00");
        heatmap.ColumnLabels.Add("18:00");
        heatmap.ColumnLabels.Add("21:00");
        double[,] matrix = {
            { 20, 45, 78, 60, 30 },
            { 35, 60, 92, 85, 40 },
            { 40, 75, 95, 80, 50 },
            { 30, 65, 88, 70, 45 },
            { 25, 50, 70, 90, 65 }
        };
        for (int r = 0; r < 5; r++)
            for (int c = 0; c < 5; c++)
                heatmap.SetCell(r, c, matrix[r, c]);

        // Tab 6: KPIs & Statistical
        _gaugeChart.Value = 76.5;
        _gaugeChart.Title = "Server Load";
        _gaugeChart.Unit = "%";

        _bulletChart.Title = "YTD Sales";
        _bulletChart.Subtitle = "$ in Thousands";
        _bulletChart.Value = 82;
        _bulletChart.Target = 88;
        _bulletChart.BadThreshold = 50;
        _bulletChart.SatisfactoryThreshold = 75;

        _sparklineChart.SetData(new double[] { 14, 18, 15, 22, 28, 25, 32, 40, 38, 48, 52 });

        var scatter = (LengkoScatterChart)_cards[28].Chart;
        var blue = Color.FromArgb(59, 130, 246);
        scatter.Points.Add(new LengkoScatterPoint(10, 25, blue, "A1"));
        scatter.Points.Add(new LengkoScatterPoint(20, 38, blue, "A2"));
        scatter.Points.Add(new LengkoScatterPoint(30, 52, blue, "A3"));
        scatter.Points.Add(new LengkoScatterPoint(40, 66, blue, "A4"));
        scatter.Points.Add(new LengkoScatterPoint(50, 80, blue, "A5"));
        var red = Color.FromArgb(239, 68, 68);
        scatter.Points.Add(new LengkoScatterPoint(12, 18, red, "B1"));
        scatter.Points.Add(new LengkoScatterPoint(22, 30, red, "B2"));
        scatter.Points.Add(new LengkoScatterPoint(32, 40, red, "B3"));
        scatter.Points.Add(new LengkoScatterPoint(42, 58, red, "B4"));
        scatter.Points.Add(new LengkoScatterPoint(52, 70, red, "B5"));

        var bubble = (LengkoBubbleChart)_cards[29].Chart;
        bubble.Points.Add(new LengkoBubblePoint(15, 45, 14, "Opportunity A", Color.FromArgb(59, 130, 246)));
        bubble.Points.Add(new LengkoBubblePoint(30, 70, 22, "Opportunity B", Color.FromArgb(16, 185, 129)));
        bubble.Points.Add(new LengkoBubblePoint(55, 35, 18, "Opportunity C", Color.FromArgb(245, 158, 11)));
        bubble.Points.Add(new LengkoBubblePoint(75, 85, 30, "Strategic Bet", Color.FromArgb(139, 92, 246)));
        bubble.Points.Add(new LengkoBubblePoint(85, 50, 16, "Core Business", Color.FromArgb(236, 72, 153)));

        var boxplot = (LengkoBoxPlotChart)_cards[30].Chart;
        boxplot.Items.Add(new LengkoBoxPlotItem("Group A", 20, 35, 50, 68, 85));
        boxplot.Items.Add(new LengkoBoxPlotItem("Group B", 30, 45, 62, 78, 92));
        boxplot.Items.Add(new LengkoBoxPlotItem("Group C", 15, 28, 42, 58, 75));

        var errorbar = (LengkoErrorBarChart)_cards[31].Chart;
        errorbar.Points.Add(new LengkoErrorPoint("Sample 1", 45, 5, 6));
        errorbar.Points.Add(new LengkoErrorPoint("Sample 2", 62, 8, 7));
        errorbar.Points.Add(new LengkoErrorPoint("Sample 3", 53, 4, 5));
        errorbar.Points.Add(new LengkoErrorPoint("Sample 4", 78, 9, 8));
    }
}
