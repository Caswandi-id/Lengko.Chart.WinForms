namespace Lengko.Chart.Demo.FSharp

open System
open System.Collections.Generic
open System.Drawing
open System.Windows.Forms
open Lengko.Chart.WinForms

type CardItem = {
    Card: Panel
    Title: Label
    Subtitle: Label
    Chart: Control
}

module Program =

    type MainForm() as this =
        inherit Form()

        let mutable isDark = false
        let rand = Random()
        let cards = List<CardItem>()

        let createToolbarButton (text: string) (bg: Color) =
            let btn = new Button(
                Text = text,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Height = 34,
                AutoSize = true,
                Padding = Padding(10, 4, 10, 4),
                Margin = Padding(0, 0, 8, 0),
                Cursor = Cursors.Hand
            )
            btn.FlatAppearance.BorderSize <- 0
            btn

        let createCard (title: string) (sub: string) (chart: Control) =
            let card = new Panel(
                Dock = DockStyle.Fill,
                Margin = Padding(6),
                Padding = Padding(14, 12, 14, 12),
                BackColor = Color.White
            )
            card.Paint.Add(fun e ->
                use pen = new Pen((if isDark then Color.FromArgb(51, 65, 85) else Color.FromArgb(226, 232, 240)), 1.0f)
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1)
            )
            let pnlHeader = new Panel(
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent
            )
            let lblTitle = new Label(
                Text = title,
                Font = new Font("Segoe UI", 10.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = Point(0, 0)
            )
            let lblSub = new Label(
                Text = sub,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = Point(0, 22)
            )
            pnlHeader.Controls.Add(lblTitle)
            pnlHeader.Controls.Add(lblSub)
            chart.Dock <- DockStyle.Fill
            card.Controls.Add(chart)
            card.Controls.Add(pnlHeader)
            pnlHeader.SendToBack()
            cards.Add({ Card = card; Title = lblTitle; Subtitle = lblSub; Chart = chart })
            card

        let createGrid (rows: int) =
            let grid = new TableLayoutPanel(
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = rows,
                AutoScroll = true,
                Padding = Padding(4)
            )
            grid.ColumnStyles.Add(ColumnStyle(SizeType.Percent, 50.0f)) |> ignore
            grid.ColumnStyles.Add(ColumnStyle(SizeType.Percent, 50.0f)) |> ignore
            for _ in 1 .. rows do
                grid.RowStyles.Add(RowStyle(SizeType.Absolute, 320.0f)) |> ignore
            grid

        do
            this.Text <- "Lengko.Chart.WinForms — F# Showcase (All 32 Charts)"
            this.Width <- 1240
            this.Height <- 880
            this.StartPosition <- FormStartPosition.CenterScreen
            this.Font <- new Font("Segoe UI", 9.5f)
            this.BackColor <- Color.FromArgb(248, 250, 252)

            let root = new TableLayoutPanel(
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = Padding(16)
            )
            root.RowStyles.Add(RowStyle(SizeType.Absolute, 56.0f)) |> ignore
            root.RowStyles.Add(RowStyle(SizeType.Absolute, 42.0f)) |> ignore
            root.RowStyles.Add(RowStyle(SizeType.Percent, 100.0f)) |> ignore
            root.RowStyles.Add(RowStyle(SizeType.Absolute, 28.0f)) |> ignore

            // 1. Header
            let header = new Panel(Dock = DockStyle.Fill)
            let lblMainTitle = new Label(
                Text = "Lengko.Chart.WinForms — F# Showcase (All 32 Charts)",
                Font = new Font("Segoe UI", 15.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = Point(0, 0)
            )
            let lblMainSub = new Label(
                Text = "Complete catalog: 32 WinForms chart controls with pure GDI+ rendering, hover tooltips, and themes.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = Point(0, 28)
            )
            header.Controls.Add(lblMainTitle)
            header.Controls.Add(lblMainSub)
            root.Controls.Add(header, 0, 0)

            // 2. Toolbar
            let toolbar = new FlowLayoutPanel(
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = Padding(0)
            )
            let btnTheme = createToolbarButton "Theme: Light" (Color.FromArgb(71, 85, 105))
            let btnDonut = createToolbarButton "Pie Mode: Standard" (Color.FromArgb(2, 132, 199))
            let btnSmooth = createToolbarButton "Line Curve: Smooth" (Color.FromArgb(5, 150, 105))
            let btnRandomize = createToolbarButton "Randomize Data" (Color.FromArgb(139, 92, 246))

            toolbar.Controls.Add(btnTheme)
            toolbar.Controls.Add(btnDonut)
            toolbar.Controls.Add(btnSmooth)
            toolbar.Controls.Add(btnRandomize)
            root.Controls.Add(toolbar, 0, 1)

            // 3. TabControl
            let tabs = new TabControl(
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = Point(14, 8)
            )

            // Specific controls referenced in buttons
            let barChart = new LengkoBarChart(ShowGrid = true, ShowValues = true)
            let hBarChart = new LengkoHorizontalBarChart(ShowValues = true)
            let lineChart = new LengkoLineChart(SmoothCurves = true, ShowPoints = true, FillArea = true, ShowGrid = true)
            let areaChart = new LengkoAreaChart(SmoothCurves = true, ShowGrid = true)
            let pieChart = new LengkoPieChart(ShowPercentages = true)
            let donutChart = new LengkoDonutChart(ShowPercentages = true)
            let radarChart = new LengkoRadarChart()
            let gaugeChart = new LengkoRadialGauge()
            let bulletChart = new LengkoBulletGraph()
            let sparklineChart = new LengkoSparkline()

            // Tab 1: Bars & Columns (6)
            let tabBars = new TabPage("Bars & Columns (6)")
            let gridBars = createGrid 3
            let stackedBar = new LengkoStackedBarChart(ShowGrid = true)
            let hist = new LengkoHistogramChart(ShowGrid = true)
            let pareto = new LengkoParetoChart()
            let waterfall = new LengkoWaterfallChart()
            gridBars.Controls.Add(createCard "Bar Chart (LengkoBarChart)" "Multi-series grouped column comparison" barChart, 0, 0)
            gridBars.Controls.Add(createCard "Horizontal Bar Chart (LengkoHorizontalBarChart)" "Ranked category comparison with horizontal bars" hBarChart, 1, 0)
            gridBars.Controls.Add(createCard "Stacked Bar Chart (LengkoStackedBarChart)" "Part-to-whole segment distribution across periods" stackedBar, 0, 1)
            gridBars.Controls.Add(createCard "Histogram Chart (LengkoHistogramChart)" "Frequency distribution across numerical range bins" hist, 1, 1)
            gridBars.Controls.Add(createCard "Pareto Chart (LengkoParetoChart)" "80/20 defect / factor analysis with cumulative line" pareto, 0, 2)
            gridBars.Controls.Add(createCard "Waterfall Chart (LengkoWaterfallChart)" "Incremental revenue, expense, and net variance flow" waterfall, 1, 2)
            tabBars.Controls.Add(gridBars)
            tabs.TabPages.Add(tabBars)

            // Tab 2: Lines & Areas (6)
            let tabLines = new TabPage("Lines & Areas (6)")
            let gridLines = createGrid 3
            let stackedArea = new LengkoStackedAreaChart(ShowGrid = true)
            let stepLine = new LengkoStepLineChart(ShowPoints = true, FillArea = true)
            let combo = new LengkoComboChart(ShowGrid = true)
            let generic = new LengkoChart(Title = "Multi-Point Metric")
            gridLines.Controls.Add(createCard "Line Chart (LengkoLineChart)" "Multi-series continuous trends with points and fill" lineChart, 0, 0)
            gridLines.Controls.Add(createCard "Area Chart (LengkoAreaChart)" "Cumulative trend volume with smooth area fill" areaChart, 1, 0)
            gridLines.Controls.Add(createCard "Stacked Area Chart (LengkoStackedAreaChart)" "Cumulative regional contribution over time" stackedArea, 0, 1)
            gridLines.Controls.Add(createCard "Step Line Chart (LengkoStepLineChart)" "Discrete step-wise inventory and threshold changes" stepLine, 1, 1)
            gridLines.Controls.Add(createCard "Combo Chart (LengkoComboChart)" "Dual-axis combination of volume bars and profit line" combo, 0, 2)
            gridLines.Controls.Add(createCard "Generic Chart (LengkoChart)" "Lightweight multi-point metric visualization" generic, 1, 2)
            tabLines.Controls.Add(gridLines)
            tabs.TabPages.Add(tabLines)

            // Tab 3: Circular & Polar (5)
            let tabCirc = new TabPage("Circular & Polar (5)")
            let gridCirc = createGrid 3
            let polar = new LengkoPolarAreaChart()
            let sunburst = new LengkoSunburstChart()
            gridCirc.Controls.Add(createCard "Pie Chart (LengkoPieChart)" "Proportional slice distribution with percentages" pieChart, 0, 0)
            gridCirc.Controls.Add(createCard "Donut Chart (LengkoDonutChart)" "Ring distribution with central readout and legend" donutChart, 1, 0)
            gridCirc.Controls.Add(createCard "Radar Chart (LengkoRadarChart)" "Multi-variable capability spider comparison" radarChart, 0, 1)
            gridCirc.Controls.Add(createCard "Polar Area Chart (LengkoPolarAreaChart)" "Cyclic and directional magnitude comparison" polar, 1, 1)
            gridCirc.Controls.Add(createCard "Sunburst Chart (LengkoSunburstChart)" "Multi-level concentric radial hierarchy visualizer" sunburst, 0, 2)
            tabCirc.Controls.Add(gridCirc)
            tabs.TabPages.Add(tabCirc)

            // Tab 4: Financial & Timelines (3)
            let tabFin = new TabPage("Financial & Timelines (3)")
            let gridFin = createGrid 3
            let candle = new LengkoCandlestickChart(ShowVolume = true)
            let ohlc = new LengkoOhlcChart()
            let gantt = new LengkoGanttChart()
            gridFin.Controls.Add(createCard "Candlestick Chart (LengkoCandlestickChart)" "Asset price action with OHLC candles and volume bars" candle, 0, 0)
            gridFin.Controls.Add(createCard "OHLC Bar Chart (LengkoOhlcChart)" "Open-High-Low-Close financial trading price bars" ohlc, 1, 0)
            gridFin.Controls.Add(createCard "Gantt Chart (LengkoGanttChart)" "Project timeline, milestone scheduling, and progress" gantt, 0, 1)
            tabFin.Controls.Add(gridFin)
            tabs.TabPages.Add(tabFin)

            // Tab 5: Hierarchy & Flow (5)
            let tabFlow = new TabPage("Hierarchy & Flow (5)")
            let gridFlow = createGrid 3
            let treemap = new LengkoTreemap()
            let sankey = new LengkoSankeyChart()
            let funnel = new LengkoFunnelChart()
            let pyramid = new LengkoPyramidChart()
            let heatmap = new LengkoHeatmapChart()
            gridFlow.Controls.Add(createCard "Treemap (LengkoTreemap)" "Hierarchical space-filling rectangular proportions" treemap, 0, 0)
            gridFlow.Controls.Add(createCard "Sankey Flow Chart (LengkoSankeyChart)" "Directed flow paths, quantities, and node transfers" sankey, 1, 0)
            gridFlow.Controls.Add(createCard "Funnel Chart (LengkoFunnelChart)" "Conversion funnel stages and pipeline drop-off" funnel, 0, 1)
            gridFlow.Controls.Add(createCard "Pyramid Chart (LengkoPyramidChart)" "Hierarchical tiered organizational structure" pyramid, 1, 1)
            gridFlow.Controls.Add(createCard "Heatmap Chart (LengkoHeatmapChart)" "2D intensity matrix grid with color spectrum" heatmap, 0, 2)
            tabFlow.Controls.Add(gridFlow)
            tabs.TabPages.Add(tabFlow)

            // Tab 6: KPIs & Statistical (7)
            let tabKpi = new TabPage("KPIs & Statistical (7)")
            let gridKpi = createGrid 4
            let scatter = new LengkoScatterChart(ShowTrendline = true)
            let bubble = new LengkoBubbleChart()
            let boxplot = new LengkoBoxPlotChart()
            let errorbar = new LengkoErrorBarChart()
            gridKpi.Controls.Add(createCard "Radial Gauge (LengkoRadialGauge)" "Circular KPI progress arc indicator with centered metrics" gaugeChart, 0, 0)
            gridKpi.Controls.Add(createCard "Bullet Graph (LengkoBulletGraph)" "Compact KPI against target and qualitative performance bands" bulletChart, 1, 0)
            gridKpi.Controls.Add(createCard "Sparkline (LengkoSparkline)" "Ultra-compact micro trend visualizer for dashboards" sparklineChart, 0, 1)
            gridKpi.Controls.Add(createCard "Scatter Chart (LengkoScatterChart)" "Bivariate correlation points with linear regression trendline" scatter, 1, 1)
            gridKpi.Controls.Add(createCard "Bubble Chart (LengkoBubbleChart)" "Three-variable correlation mapping X, Y, and magnitude" bubble, 0, 2)
            gridKpi.Controls.Add(createCard "Box Plot Chart (LengkoBoxPlotChart)" "Five-number summary showing min, quartiles, and extremes" boxplot, 1, 2)
            gridKpi.Controls.Add(createCard "Error Bar Chart (LengkoErrorBarChart)" "Mean values with upper and lower measurement uncertainty" errorbar, 0, 3)
            tabKpi.Controls.Add(gridKpi)
            tabs.TabPages.Add(tabKpi)

            root.Controls.Add(tabs, 0, 2)

            // 4. Footer Tip
            let footer = new Label(
                Text = "💡 Tips: Hover over chart items for interactive tooltips. Scroll wheel to zoom, drag mouse to pan. Click 'Theme' to toggle dark mode.",
                Font = new Font("Segoe UI", 9.0f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            )
            root.Controls.Add(footer, 0, 3)

            this.Controls.Add(root)

            // Wire events
            btnTheme.Click.Add(fun _ ->
                isDark <- not isDark
                btnTheme.Text <- if isDark then "Theme: Dark" else "Theme: Light"
                let theme = if isDark then LengkoChartTheme.Dark else LengkoChartTheme.Light
                LengkoChartThemeScope.DefaultTheme <- theme

                let bg = if isDark then Color.FromArgb(15, 23, 42) else Color.FromArgb(248, 250, 252)
                let cardBg = if isDark then Color.FromArgb(30, 41, 59) else Color.White
                let textPrimary = if isDark then Color.FromArgb(241, 245, 249) else Color.FromArgb(15, 23, 42)
                let textSecondary = if isDark then Color.FromArgb(148, 163, 184) else Color.FromArgb(100, 116, 139)

                this.BackColor <- bg
                lblMainTitle.ForeColor <- textPrimary
                lblMainSub.ForeColor <- textSecondary
                footer.ForeColor <- textSecondary

                for c in cards do
                    c.Card.BackColor <- cardBg
                    c.Title.ForeColor <- textPrimary
                    c.Subtitle.ForeColor <- textSecondary
                    c.Chart.BackColor <- cardBg
                    c.Card.Invalidate()
                    match box c.Chart with
                    | :? ILengkoChartThemedControl as themed -> themed.ApplyTheme(theme)
                    | _ -> c.Chart.Invalidate()
            )

            btnDonut.Click.Add(fun _ ->
                pieChart.IsDonut <- not pieChart.IsDonut
                btnDonut.Text <- if pieChart.IsDonut then "Pie Mode: Donut" else "Pie Mode: Standard"
            )

            btnSmooth.Click.Add(fun _ ->
                lineChart.SmoothCurves <- not lineChart.SmoothCurves
                areaChart.SmoothCurves <- not areaChart.SmoothCurves
                btnSmooth.Text <- if lineChart.SmoothCurves then "Line Curve: Smooth" else "Line Curve: Straight"
            )

            btnRandomize.Click.Add(fun _ ->
                for srs in barChart.Series do
                    for i in 0 .. srs.Values.Count - 1 do
                        srs.Values.[i] <- float (rand.Next(30, 260))
                barChart.Invalidate()

                for it in hBarChart.Items do
                    it.Value <- float (rand.Next(40, 100))
                hBarChart.Invalidate()

                for srs in lineChart.Series do
                    for i in 0 .. srs.Values.Count - 1 do
                        srs.Values.[i] <- float (rand.Next(15, 110))
                lineChart.Invalidate()

                for slc in pieChart.Slices do
                    slc.Value <- float (rand.Next(10, 60))
                pieChart.Invalidate()

                for slc in donutChart.Slices do
                    slc.Value <- float (rand.Next(10, 60))
                donutChart.Invalidate()

                for srs in radarChart.Series do
                    for i in 0 .. srs.Values.Count - 1 do
                        srs.Values.[i] <- float (rand.Next(40, 99))
                radarChart.Invalidate()

                gaugeChart.Value <- float (rand.Next(20, 98))
                bulletChart.Value <- float (rand.Next(40, 95))

                let sparkData = [| for _ in 1 .. 11 -> float (rand.Next(10, 65)) |]
                sparklineChart.SetData(sparkData)
            )

            // Populate initial sample data
            // Tab 1: Bars & Columns
            barChart.Categories.Add("Q1")
            barChart.Categories.Add("Q2")
            barChart.Categories.Add("Q3")
            barChart.Categories.Add("Q4")
            barChart.Series.Add(LengkoBarSeries("Revenue", [| 120.0; 150.0; 180.0; 220.0 |], Color.FromArgb(59, 130, 246)))
            barChart.Series.Add(LengkoBarSeries("Profit", [| 45.0; 60.0; 75.0; 95.0 |], Color.FromArgb(16, 185, 129)))

            hBarChart.AddItem("TypeScript", 88.0, Color.FromArgb(59, 130, 246))
            hBarChart.AddItem("C# / .NET", 94.0, Color.FromArgb(139, 92, 246))
            hBarChart.AddItem("Python", 82.0, Color.FromArgb(245, 158, 11))
            hBarChart.AddItem("Go", 75.0, Color.FromArgb(6, 182, 212))
            hBarChart.AddItem("Rust", 70.0, Color.FromArgb(239, 68, 68))

            stackedBar.Categories.Add("2021")
            stackedBar.Categories.Add("2022")
            stackedBar.Categories.Add("2023")
            stackedBar.Categories.Add("2024")
            stackedBar.Series.Add(LengkoBarSeries("Cloud", [| 40.0; 55.0; 75.0; 110.0 |], Color.FromArgb(59, 130, 246)))
            stackedBar.Series.Add(LengkoBarSeries("On-Prem", [| 60.0; 50.0; 45.0; 30.0 |], Color.FromArgb(16, 185, 129)))
            stackedBar.Series.Add(LengkoBarSeries("Support", [| 20.0; 25.0; 30.0; 35.0 |], Color.FromArgb(245, 158, 11)))

            hist.SetData([| 18.0; 22.0; 25.0; 27.0; 30.0; 32.0; 34.0; 35.0; 38.0; 40.0; 42.0; 44.0; 45.0; 48.0; 52.0; 55.0; 60.0; 65.0; 72.0 |], 6)

            pareto.Items.Add(LengkoParetoItem("Timeout 504", 142.0))
            pareto.Items.Add(LengkoParetoItem("Auth 401", 95.0))
            pareto.Items.Add(LengkoParetoItem("NullRef", 48.0))
            pareto.Items.Add(LengkoParetoItem("DB Lock", 24.0))
            pareto.Items.Add(LengkoParetoItem("Bad Format", 12.0))

            waterfall.Items.Add(LengkoWaterfallItem("Start Cash", 120.0, isTotal = true))
            waterfall.Items.Add(LengkoWaterfallItem("Sales", 52.0))
            waterfall.Items.Add(LengkoWaterfallItem("Subscriptions", 30.0))
            waterfall.Items.Add(LengkoWaterfallItem("Salaries", -42.0))
            waterfall.Items.Add(LengkoWaterfallItem("Cloud", -18.0))
            waterfall.Items.Add(LengkoWaterfallItem("Marketing", -14.0))
            waterfall.Items.Add(LengkoWaterfallItem("Net Cash", 0.0, isTotal = true))

            // Tab 2: Lines & Areas
            lineChart.Series.Add(LengkoLineSeries("Traffic (k)", [| 22.0; 34.0; 45.0; 40.0; 58.0; 65.0; 80.0 |], Color.FromArgb(99, 102, 241)))
            lineChart.Series.Add(LengkoLineSeries("Signups", [| 10.0; 18.0; 24.0; 28.0; 35.0; 42.0; 55.0 |], Color.FromArgb(236, 72, 153)))

            areaChart.Labels.Add("Jan")
            areaChart.Labels.Add("Feb")
            areaChart.Labels.Add("Mar")
            areaChart.Labels.Add("Apr")
            areaChart.Labels.Add("May")
            areaChart.Labels.Add("Jun")
            areaChart.Series.Add(LengkoAreaSeries("Inbound", [| 30.0; 45.0; 52.0; 68.0; 74.0; 90.0 |], Color.FromArgb(59, 130, 246)))
            areaChart.Series.Add(LengkoAreaSeries("Outbound", [| 18.0; 25.0; 32.0; 40.0; 48.0; 62.0 |], Color.FromArgb(16, 185, 129)))

            stackedArea.Categories.Add("Jan")
            stackedArea.Categories.Add("Feb")
            stackedArea.Categories.Add("Mar")
            stackedArea.Categories.Add("Apr")
            stackedArea.Categories.Add("May")
            stackedArea.Categories.Add("Jun")
            stackedArea.Series.Add(LengkoAreaSeries("North", [| 25.0; 30.0; 38.0; 45.0; 52.0; 60.0 |], Color.FromArgb(14, 165, 233)))
            stackedArea.Series.Add(LengkoAreaSeries("South", [| 20.0; 24.0; 30.0; 36.0; 42.0; 48.0 |], Color.FromArgb(168, 85, 247)))
            stackedArea.Series.Add(LengkoAreaSeries("East", [| 15.0; 18.0; 22.0; 28.0; 32.0; 38.0 |], Color.FromArgb(249, 115, 22)))

            stepLine.Series.Add(LengkoStepSeries("Inventory", [| 100.0; 85.0; 85.0; 60.0; 120.0; 95.0; 70.0 |], Color.FromArgb(16, 185, 129)))
            stepLine.Series.Add(LengkoStepSeries("Threshold", [| 50.0; 50.0; 50.0; 50.0; 50.0; 50.0; 50.0 |], Color.FromArgb(239, 68, 68)))

            combo.Categories.Add("Q1")
            combo.Categories.Add("Q2")
            combo.Categories.Add("Q3")
            combo.Categories.Add("Q4")
            combo.BarSeries.Add(LengkoBarSeries("Units Sold", [| 320.0; 450.0; 580.0; 710.0 |], Color.FromArgb(59, 130, 246)))
            combo.LineSeries.Add(LengkoLineSeries("Margin %", [| 22.0; 28.0; 26.0; 31.0 |], Color.FromArgb(239, 68, 68)))

            generic.AddPoint("CPU", 48.0)
            generic.AddPoint("RAM", 72.0)
            generic.AddPoint("Disk", 60.0)
            generic.AddPoint("Network", 35.0)
            generic.AddPoint("GPU", 84.0)

            // Tab 3: Circular & Polar
            pieChart.Slices.Add(LengkoPieSlice("North America", 38.0, Color.FromArgb(59, 130, 246)))
            pieChart.Slices.Add(LengkoPieSlice("Europe", 27.0, Color.FromArgb(16, 185, 129)))
            pieChart.Slices.Add(LengkoPieSlice("Asia Pacific", 23.0, Color.FromArgb(245, 158, 11)))
            pieChart.Slices.Add(LengkoPieSlice("Latin America", 12.0, Color.FromArgb(239, 68, 68)))

            donutChart.Slices.Add(LengkoPieSlice("Enterprise", 45.0, Color.FromArgb(99, 102, 241)))
            donutChart.Slices.Add(LengkoPieSlice("SMB", 30.0, Color.FromArgb(236, 72, 153)))
            donutChart.Slices.Add(LengkoPieSlice("Startup", 15.0, Color.FromArgb(20, 184, 166)))
            donutChart.Slices.Add(LengkoPieSlice("Consumer", 10.0, Color.FromArgb(245, 158, 11)))

            radarChart.Axes.Add("Speed")
            radarChart.Axes.Add("Reliability")
            radarChart.Axes.Add("Usability")
            radarChart.Axes.Add("Security")
            radarChart.Axes.Add("Scalability")
            radarChart.Series.Add(LengkoRadarSeries("Lengko UI", [| 92.0; 95.0; 88.0; 90.0; 94.0 |], Color.FromArgb(139, 92, 246)))
            radarChart.Series.Add(LengkoRadarSeries("Standard", [| 65.0; 70.0; 60.0; 75.0; 68.0 |], Color.FromArgb(148, 163, 184)))

            polar.Items.Add(LengkoPolarItem("North", 45.0, Color.FromArgb(59, 130, 246)))
            polar.Items.Add(LengkoPolarItem("NE", 60.0, Color.FromArgb(99, 102, 241)))
            polar.Items.Add(LengkoPolarItem("East", 80.0, Color.FromArgb(139, 92, 246)))
            polar.Items.Add(LengkoPolarItem("SE", 50.0, Color.FromArgb(236, 72, 153)))
            polar.Items.Add(LengkoPolarItem("South", 35.0, Color.FromArgb(244, 63, 94)))
            polar.Items.Add(LengkoPolarItem("SW", 55.0, Color.FromArgb(245, 158, 11)))
            polar.Items.Add(LengkoPolarItem("West", 75.0, Color.FromArgb(16, 185, 129)))
            polar.Items.Add(LengkoPolarItem("NW", 40.0, Color.FromArgb(20, 184, 166)))

            sunburst.Items.Add(LengkoSunburstItem("Software", 60.0, 0, Color.FromArgb(59, 130, 246)))
            sunburst.Items.Add(LengkoSunburstItem("Hardware", 40.0, 0, Color.FromArgb(16, 185, 129)))
            sunburst.Items.Add(LengkoSunburstItem("Desktop", 35.0, 1, Color.FromArgb(99, 102, 241)))
            sunburst.Items.Add(LengkoSunburstItem("Mobile", 25.0, 1, Color.FromArgb(14, 165, 233)))
            sunburst.Items.Add(LengkoSunburstItem("Servers", 25.0, 1, Color.FromArgb(34, 197, 94)))
            sunburst.Items.Add(LengkoSunburstItem("IoT", 15.0, 1, Color.FromArgb(245, 158, 11)))

            // Tab 4: Financial & Timelines
            candle.AddCandle("09:30", 150.2, 153.5, 149.8, 152.8, 142000.0)
            candle.AddCandle("10:00", 152.8, 154.0, 151.2, 151.6, 98000.0)
            candle.AddCandle("10:30", 151.6, 155.2, 151.0, 154.5, 175000.0)
            candle.AddCandle("11:00", 154.5, 156.0, 153.8, 155.8, 110000.0)
            candle.AddCandle("11:30", 155.8, 156.2, 153.0, 153.4, 130000.0)

            ohlc.Bars.Add(LengkoOhlcBar("Mon", 100.0, 108.0, 98.0, 105.0))
            ohlc.Bars.Add(LengkoOhlcBar("Tue", 105.0, 110.0, 102.0, 104.0))
            ohlc.Bars.Add(LengkoOhlcBar("Wed", 104.0, 114.0, 103.0, 112.0))
            ohlc.Bars.Add(LengkoOhlcBar("Thu", 112.0, 115.0, 108.0, 109.0))
            ohlc.Bars.Add(LengkoOhlcBar("Fri", 109.0, 118.0, 107.0, 116.0))

            let baseDate = DateTime.Today
            gantt.Tasks.Add(LengkoGanttItem("Architecture & API", baseDate, baseDate.AddDays(5.0), 100))
            gantt.Tasks.Add(LengkoGanttItem("Core Chart Engine", baseDate.AddDays(3.0), baseDate.AddDays(10.0), 80))
            gantt.Tasks.Add(LengkoGanttItem("32 WinForms Controls", baseDate.AddDays(6.0), baseDate.AddDays(15.0), 55))
            gantt.Tasks.Add(LengkoGanttItem("Testing & Packaging", baseDate.AddDays(12.0), baseDate.AddDays(18.0), 20))

            // Tab 5: Hierarchy & Flow
            treemap.Items.Add(LengkoTreemapItem("Finance", 420.0))
            treemap.Items.Add(LengkoTreemapItem("Engineering", 350.0))
            treemap.Items.Add(LengkoTreemapItem("Marketing", 220.0))
            treemap.Items.Add(LengkoTreemapItem("Support", 160.0))
            treemap.Items.Add(LengkoTreemapItem("Legal & HR", 90.0))

            sankey.Links.Add(LengkoSankeyLink("Organic Search", "Landing Page", 140.0))
            sankey.Links.Add(LengkoSankeyLink("Paid Ads", "Landing Page", 90.0))
            sankey.Links.Add(LengkoSankeyLink("Referral", "Product Specs", 65.0))
            sankey.Links.Add(LengkoSankeyLink("Landing Page", "Pricing", 130.0))
            sankey.Links.Add(LengkoSankeyLink("Landing Page", "Checkout", 70.0))
            sankey.Links.Add(LengkoSankeyLink("Pricing", "Checkout", 100.0))
            sankey.Links.Add(LengkoSankeyLink("Product Specs", "Checkout", 45.0))

            funnel.Stages.Add(LengkoFunnelStage("Website Visits", 12500.0))
            funnel.Stages.Add(LengkoFunnelStage("Sign Up", 5200.0))
            funnel.Stages.Add(LengkoFunnelStage("Cart Added", 2800.0))
            funnel.Stages.Add(LengkoFunnelStage("Checkout Started", 1600.0))
            funnel.Stages.Add(LengkoFunnelStage("Purchased", 1100.0))

            pyramid.Stages.Add(LengkoPyramidStage("Executives", 8.0))
            pyramid.Stages.Add(LengkoPyramidStage("Directors", 24.0))
            pyramid.Stages.Add(LengkoPyramidStage("Team Leads", 65.0))
            pyramid.Stages.Add(LengkoPyramidStage("Specialists", 180.0))
            pyramid.Stages.Add(LengkoPyramidStage("Associates", 340.0))

            heatmap.RowLabels.Add("Mon")
            heatmap.RowLabels.Add("Tue")
            heatmap.RowLabels.Add("Wed")
            heatmap.RowLabels.Add("Thu")
            heatmap.RowLabels.Add("Fri")
            heatmap.ColumnLabels.Add("09:00")
            heatmap.ColumnLabels.Add("12:00")
            heatmap.ColumnLabels.Add("15:00")
            heatmap.ColumnLabels.Add("18:00")
            heatmap.ColumnLabels.Add("21:00")
            let matrix: float[,] = array2D [
                [ 20.0; 45.0; 78.0; 60.0; 30.0 ]
                [ 35.0; 60.0; 92.0; 85.0; 40.0 ]
                [ 40.0; 75.0; 95.0; 80.0; 50.0 ]
                [ 30.0; 65.0; 88.0; 70.0; 45.0 ]
                [ 25.0; 50.0; 70.0; 90.0; 65.0 ]
            ]
            for r in 0 .. 4 do
                for c in 0 .. 4 do
                    heatmap.SetCell(r, c, matrix.[r, c])

            // Tab 6: KPIs & Statistical
            gaugeChart.Value <- 76.5
            gaugeChart.Title <- "Server Load"
            gaugeChart.Unit <- "%"

            bulletChart.Title <- "YTD Sales"
            bulletChart.Subtitle <- "$ in Thousands"
            bulletChart.Value <- 82.0
            bulletChart.Target <- 88.0
            bulletChart.BadThreshold <- 50.0
            bulletChart.SatisfactoryThreshold <- 75.0

            sparklineChart.SetData([| 14.0; 18.0; 15.0; 22.0; 28.0; 25.0; 32.0; 40.0; 38.0; 48.0; 52.0 |])

            let blue = Color.FromArgb(59, 130, 246)
            scatter.Points.Add(LengkoScatterPoint(10.0, 25.0, blue, "A1"))
            scatter.Points.Add(LengkoScatterPoint(20.0, 38.0, blue, "A2"))
            scatter.Points.Add(LengkoScatterPoint(30.0, 52.0, blue, "A3"))
            scatter.Points.Add(LengkoScatterPoint(40.0, 66.0, blue, "A4"))
            scatter.Points.Add(LengkoScatterPoint(50.0, 80.0, blue, "A5"))
            let red = Color.FromArgb(239, 68, 68)
            scatter.Points.Add(LengkoScatterPoint(12.0, 18.0, red, "B1"))
            scatter.Points.Add(LengkoScatterPoint(22.0, 30.0, red, "B2"))
            scatter.Points.Add(LengkoScatterPoint(32.0, 40.0, red, "B3"))
            scatter.Points.Add(LengkoScatterPoint(42.0, 58.0, red, "B4"))
            scatter.Points.Add(LengkoScatterPoint(52.0, 70.0, red, "B5"))

            bubble.Points.Add(LengkoBubblePoint(15.0, 45.0, 14.0, "Opportunity A", Color.FromArgb(59, 130, 246)))
            bubble.Points.Add(LengkoBubblePoint(30.0, 70.0, 22.0, "Opportunity B", Color.FromArgb(16, 185, 129)))
            bubble.Points.Add(LengkoBubblePoint(55.0, 35.0, 18.0, "Opportunity C", Color.FromArgb(245, 158, 11)))
            bubble.Points.Add(LengkoBubblePoint(75.0, 85.0, 30.0, "Strategic Bet", Color.FromArgb(139, 92, 246)))
            bubble.Points.Add(LengkoBubblePoint(85.0, 50.0, 16.0, "Core Business", Color.FromArgb(236, 72, 153)))

            boxplot.Items.Add(LengkoBoxPlotItem("Group A", 20.0, 35.0, 50.0, 68.0, 85.0))
            boxplot.Items.Add(LengkoBoxPlotItem("Group B", 30.0, 45.0, 62.0, 78.0, 92.0))
            boxplot.Items.Add(LengkoBoxPlotItem("Group C", 15.0, 28.0, 42.0, 58.0, 75.0))

            errorbar.Points.Add(LengkoErrorPoint("Sample 1", 45.0, 5.0, 6.0))
            errorbar.Points.Add(LengkoErrorPoint("Sample 2", 62.0, 8.0, 7.0))
            errorbar.Points.Add(LengkoErrorPoint("Sample 3", 53.0, 4.0, 5.0))
            errorbar.Points.Add(LengkoErrorPoint("Sample 4", 78.0, 9.0, 8.0))

    [<STAThread>]
    [<EntryPoint>]
    let main _ =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)
        Application.Run(new MainForm())
        0
