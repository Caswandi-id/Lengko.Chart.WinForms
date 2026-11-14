Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports Lengko.Chart.WinForms

Namespace Lengko.Chart.Demo.VB

    Public Class MainForm
        Inherits Form

        Private Class CardItem
            Public Property Card As Panel
            Public Property Title As Label
            Public Property Subtitle As Label
            Public Property Chart As Control
            Public Sub New(c As Panel, t As Label, s As Label, ch As Control)
                Card = c
                Title = t
                Subtitle = s
                Chart = ch
            End Sub
        End Class

        Private _isDark As Boolean = False
        Private ReadOnly _rand As New Random()
        Private ReadOnly _cards As New List(Of CardItem)()

        Private _barChart As LengkoBarChart
        Private _hBarChart As LengkoHorizontalBarChart
        Private _lineChart As LengkoLineChart
        Private _areaChart As LengkoAreaChart
        Private _pieChart As LengkoPieChart
        Private _donutChart As LengkoDonutChart
        Private _radarChart As LengkoRadarChart
        Private _gaugeChart As LengkoRadialGauge
        Private _bulletChart As LengkoBulletGraph
        Private _sparklineChart As LengkoSparkline

        Public Sub New()
            InitializeUi()
            PopulateAllCharts()
        End Sub

        Private Sub InitializeUi()
            Text = "Lengko.Chart.WinForms — VB.NET Showcase (All 32 Charts)"
            Width = 1240
            Height = 880
            StartPosition = FormStartPosition.CenterScreen
            Font = New Font("Segoe UI", 9.5F)
            BackColor = Color.FromArgb(248, 250, 252)

            Dim root As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .ColumnCount = 1,
                .RowCount = 4,
                .Padding = New Padding(16)
            }
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 42.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))

            ' 1. Header
            Dim header As New Panel With {.Dock = DockStyle.Fill}
            Dim title As New Label With {
                .Text = "Lengko.Chart.WinForms — VB.NET Showcase (All 32 Charts)",
                .Font = New Font("Segoe UI", 15.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(15, 23, 42),
                .AutoSize = True,
                .Location = New Point(0, 0)
            }
            Dim subTitle As New Label With {
                .Text = "Complete catalog: 32 WinForms chart controls with pure GDI+ rendering, hover tooltips, and themes.",
                .Font = New Font("Segoe UI", 9.5F),
                .ForeColor = Color.FromArgb(100, 116, 139),
                .AutoSize = True,
                .Location = New Point(0, 28)
            }
            header.Controls.Add(title)
            header.Controls.Add(subTitle)
            root.Controls.Add(header, 0, 0)

            ' 2. Toolbar
            Dim toolbar As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .Margin = New Padding(0)
            }
            Dim btnTheme = CreateToolbarButton("Theme: Light", Color.FromArgb(71, 85, 105))
            Dim btnDonut = CreateToolbarButton("Pie Mode: Standard", Color.FromArgb(2, 132, 199))
            Dim btnSmooth = CreateToolbarButton("Line Curve: Smooth", Color.FromArgb(5, 150, 105))
            Dim btnRandomize = CreateToolbarButton("Randomize Data", Color.FromArgb(139, 92, 246))

            toolbar.Controls.Add(btnTheme)
            toolbar.Controls.Add(btnDonut)
            toolbar.Controls.Add(btnSmooth)
            toolbar.Controls.Add(btnRandomize)
            root.Controls.Add(toolbar, 0, 1)

            ' 3. TabControl
            Dim tabs As New TabControl With {
                .Dock = DockStyle.Fill,
                .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
                .Padding = New Point(14, 8)
            }

            ' Tab 1: Bars & Columns (6)
            Dim tabBars As New TabPage("Bars & Columns (6)")
            Dim gridBars = CreateGrid(3)
            _barChart = New LengkoBarChart With {.ShowGrid = True, .ShowValues = True}
            _hBarChart = New LengkoHorizontalBarChart With {.ShowValues = True}
            Dim stackedBar As New LengkoStackedBarChart With {.ShowGrid = True}
            Dim hist As New LengkoHistogramChart With {.ShowGrid = True}
            Dim pareto As New LengkoParetoChart()
            Dim waterfall As New LengkoWaterfallChart()
            gridBars.Controls.Add(CreateCard("Bar Chart (LengkoBarChart)", "Multi-series grouped column comparison", _barChart), 0, 0)
            gridBars.Controls.Add(CreateCard("Horizontal Bar Chart (LengkoHorizontalBarChart)", "Ranked category comparison with horizontal bars", _hBarChart), 1, 0)
            gridBars.Controls.Add(CreateCard("Stacked Bar Chart (LengkoStackedBarChart)", "Part-to-whole segment distribution across periods", stackedBar), 0, 1)
            gridBars.Controls.Add(CreateCard("Histogram Chart (LengkoHistogramChart)", "Frequency distribution across numerical range bins", hist), 1, 1)
            gridBars.Controls.Add(CreateCard("Pareto Chart (LengkoParetoChart)", "80/20 defect / factor analysis with cumulative line", pareto), 0, 2)
            gridBars.Controls.Add(CreateCard("Waterfall Chart (LengkoWaterfallChart)", "Incremental revenue, expense, and net variance flow", waterfall), 1, 2)
            tabBars.Controls.Add(gridBars)
            tabs.TabPages.Add(tabBars)

            ' Tab 2: Lines & Areas (6)
            Dim tabLines As New TabPage("Lines & Areas (6)")
            Dim gridLines = CreateGrid(3)
            _lineChart = New LengkoLineChart With {.SmoothCurves = True, .ShowPoints = True, .FillArea = True, .ShowGrid = True}
            _areaChart = New LengkoAreaChart With {.SmoothCurves = True, .ShowGrid = True}
            Dim stackedArea As New LengkoStackedAreaChart With {.ShowGrid = True}
            Dim stepLine As New LengkoStepLineChart With {.ShowPoints = True, .FillArea = True}
            Dim combo As New LengkoComboChart With {.ShowGrid = True}
            Dim generic As New LengkoChart With {.Title = "Multi-Point Metric"}
            gridLines.Controls.Add(CreateCard("Line Chart (LengkoLineChart)", "Multi-series continuous trends with points and fill", _lineChart), 0, 0)
            gridLines.Controls.Add(CreateCard("Area Chart (LengkoAreaChart)", "Cumulative trend volume with smooth area fill", _areaChart), 1, 0)
            gridLines.Controls.Add(CreateCard("Stacked Area Chart (LengkoStackedAreaChart)", "Cumulative regional contribution over time", stackedArea), 0, 1)
            gridLines.Controls.Add(CreateCard("Step Line Chart (LengkoStepLineChart)", "Discrete step-wise inventory and threshold changes", stepLine), 1, 1)
            gridLines.Controls.Add(CreateCard("Combo Chart (LengkoComboChart)", "Dual-axis combination of volume bars and profit line", combo), 0, 2)
            gridLines.Controls.Add(CreateCard("Generic Chart (LengkoChart)", "Lightweight multi-point metric visualization", generic), 1, 2)
            tabLines.Controls.Add(gridLines)
            tabs.TabPages.Add(tabLines)

            ' Tab 3: Circular & Polar (5)
            Dim tabCirc As New TabPage("Circular & Polar (5)")
            Dim gridCirc = CreateGrid(3)
            _pieChart = New LengkoPieChart With {.ShowPercentages = True}
            _donutChart = New LengkoDonutChart With {.ShowPercentages = True}
            _radarChart = New LengkoRadarChart()
            Dim polar As New LengkoPolarAreaChart()
            Dim sunburst As New LengkoSunburstChart()
            gridCirc.Controls.Add(CreateCard("Pie Chart (LengkoPieChart)", "Proportional slice distribution with percentages", _pieChart), 0, 0)
            gridCirc.Controls.Add(CreateCard("Donut Chart (LengkoDonutChart)", "Ring distribution with central readout and legend", _donutChart), 1, 0)
            gridCirc.Controls.Add(CreateCard("Radar Chart (LengkoRadarChart)", "Multi-variable capability spider comparison", _radarChart), 0, 1)
            gridCirc.Controls.Add(CreateCard("Polar Area Chart (LengkoPolarAreaChart)", "Cyclic and directional magnitude comparison", polar), 1, 1)
            gridCirc.Controls.Add(CreateCard("Sunburst Chart (LengkoSunburstChart)", "Multi-level concentric radial hierarchy visualizer", sunburst), 0, 2)
            tabCirc.Controls.Add(gridCirc)
            tabs.TabPages.Add(tabCirc)

            ' Tab 4: Financial & Timelines (3)
            Dim tabFin As New TabPage("Financial & Timelines (3)")
            Dim gridFin = CreateGrid(3)
            Dim candle As New LengkoCandlestickChart With {.ShowVolume = True}
            Dim ohlc As New LengkoOhlcChart()
            Dim gantt As New LengkoGanttChart()
            gridFin.Controls.Add(CreateCard("Candlestick Chart (LengkoCandlestickChart)", "Asset price action with OHLC candles and volume bars", candle), 0, 0)
            gridFin.Controls.Add(CreateCard("OHLC Bar Chart (LengkoOhlcChart)", "Open-High-Low-Close financial trading price bars", ohlc), 1, 0)
            gridFin.Controls.Add(CreateCard("Gantt Chart (LengkoGanttChart)", "Project timeline, milestone scheduling, and progress", gantt), 0, 1)
            tabFin.Controls.Add(gridFin)
            tabs.TabPages.Add(tabFin)

            ' Tab 5: Hierarchy & Flow (5)
            Dim tabFlow As New TabPage("Hierarchy & Flow (5)")
            Dim gridFlow = CreateGrid(3)
            Dim treemap As New LengkoTreemap()
            Dim sankey As New LengkoSankeyChart()
            Dim funnel As New LengkoFunnelChart()
            Dim pyramid As New LengkoPyramidChart()
            Dim heatmap As New LengkoHeatmapChart()
            gridFlow.Controls.Add(CreateCard("Treemap (LengkoTreemap)", "Hierarchical space-filling rectangular proportions", treemap), 0, 0)
            gridFlow.Controls.Add(CreateCard("Sankey Flow Chart (LengkoSankeyChart)", "Directed flow paths, quantities, and node transfers", sankey), 1, 0)
            gridFlow.Controls.Add(CreateCard("Funnel Chart (LengkoFunnelChart)", "Conversion funnel stages and pipeline drop-off", funnel), 0, 1)
            gridFlow.Controls.Add(CreateCard("Pyramid Chart (LengkoPyramidChart)", "Hierarchical tiered organizational structure", pyramid), 1, 1)
            gridFlow.Controls.Add(CreateCard("Heatmap Chart (LengkoHeatmapChart)", "2D intensity matrix grid with color spectrum", heatmap), 0, 2)
            tabFlow.Controls.Add(gridFlow)
            tabs.TabPages.Add(tabFlow)

            ' Tab 6: KPIs & Statistical (7)
            Dim tabKpi As New TabPage("KPIs & Statistical (7)")
            Dim gridKpi = CreateGrid(4)
            _gaugeChart = New LengkoRadialGauge()
            _bulletChart = New LengkoBulletGraph()
            _sparklineChart = New LengkoSparkline()
            Dim scatter As New LengkoScatterChart With {.ShowTrendline = True}
            Dim bubble As New LengkoBubbleChart()
            Dim boxplot As New LengkoBoxPlotChart()
            Dim errorbar As New LengkoErrorBarChart()
            gridKpi.Controls.Add(CreateCard("Radial Gauge (LengkoRadialGauge)", "Circular KPI progress arc indicator with centered metrics", _gaugeChart), 0, 0)
            gridKpi.Controls.Add(CreateCard("Bullet Graph (LengkoBulletGraph)", "Compact KPI against target and qualitative performance bands", _bulletChart), 1, 0)
            gridKpi.Controls.Add(CreateCard("Sparkline (LengkoSparkline)", "Ultra-compact micro trend visualizer for dashboards", _sparklineChart), 0, 1)
            gridKpi.Controls.Add(CreateCard("Scatter Chart (LengkoScatterChart)", "Bivariate correlation points with linear regression trendline", scatter), 1, 1)
            gridKpi.Controls.Add(CreateCard("Bubble Chart (LengkoBubbleChart)", "Three-variable correlation mapping X, Y, and magnitude", bubble), 0, 2)
            gridKpi.Controls.Add(CreateCard("Box Plot Chart (LengkoBoxPlotChart)", "Five-number summary showing min, quartiles, and extremes", boxplot), 1, 2)
            gridKpi.Controls.Add(CreateCard("Error Bar Chart (LengkoErrorBarChart)", "Mean values with upper and lower measurement uncertainty", errorbar), 0, 3)
            tabKpi.Controls.Add(gridKpi)
            tabs.TabPages.Add(tabKpi)

            root.Controls.Add(tabs, 0, 2)

            ' 4. Footer Tip
            Dim footer As New Label With {
                .Text = "💡 Tips: Hover over chart items for interactive tooltips. Scroll wheel to zoom, drag mouse to pan. Click 'Theme' to toggle dark mode.",
                .Font = New Font("Segoe UI", 9.0F),
                .ForeColor = Color.FromArgb(100, 116, 139),
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            root.Controls.Add(footer, 0, 3)

            Controls.Add(root)

            ' Toolbar Events
            AddHandler btnTheme.Click, Sub(s, e)
                _isDark = Not _isDark
                btnTheme.Text = If(_isDark, "Theme: Dark", "Theme: Light")
                Dim theme = If(_isDark, LengkoChartTheme.Dark, LengkoChartTheme.Light)
                LengkoChartThemeScope.DefaultTheme = theme

                Dim bg = If(_isDark, Color.FromArgb(15, 23, 42), Color.FromArgb(248, 250, 252))
                Dim cardBg = If(_isDark, Color.FromArgb(30, 41, 59), Color.White)
                Dim textPrimary = If(_isDark, Color.FromArgb(241, 245, 249), Color.FromArgb(15, 23, 42))
                Dim textSecondary = If(_isDark, Color.FromArgb(148, 163, 184), Color.FromArgb(100, 116, 139))

                BackColor = bg
                title.ForeColor = textPrimary
                subTitle.ForeColor = textSecondary
                footer.ForeColor = textSecondary

                For Each c In _cards
                    c.Card.BackColor = cardBg
                    c.Title.ForeColor = textPrimary
                    c.Subtitle.ForeColor = textSecondary
                    c.Chart.BackColor = cardBg
                    c.Card.Invalidate()
                    Dim themed = TryCast(c.Chart, ILengkoChartThemedControl)
                    If themed IsNot Nothing Then
                        themed.ApplyTheme(theme)
                    Else
                        c.Chart.Invalidate()
                    End If
                Next
            End Sub

            AddHandler btnDonut.Click, Sub(s, e)
                _pieChart.IsDonut = Not _pieChart.IsDonut
                btnDonut.Text = If(_pieChart.IsDonut, "Pie Mode: Donut", "Pie Mode: Standard")
            End Sub

            AddHandler btnSmooth.Click, Sub(s, e)
                _lineChart.SmoothCurves = Not _lineChart.SmoothCurves
                _areaChart.SmoothCurves = Not _areaChart.SmoothCurves
                btnSmooth.Text = If(_lineChart.SmoothCurves, "Line Curve: Smooth", "Line Curve: Straight")
            End Sub

            AddHandler btnRandomize.Click, Sub(s, e)
                For Each srs In _barChart.Series
                    For i As Integer = 0 To srs.Values.Count - 1
                        srs.Values(i) = _rand.Next(30, 260)
                    Next
                Next
                _barChart.Invalidate()

                For Each it In _hBarChart.Items
                    it.Value = _rand.Next(40, 100)
                Next
                _hBarChart.Invalidate()

                For Each srs In _lineChart.Series
                    For i As Integer = 0 To srs.Values.Count - 1
                        srs.Values(i) = _rand.Next(15, 110)
                    Next
                Next
                _lineChart.Invalidate()

                For Each slc In _pieChart.Slices
                    slc.Value = _rand.Next(10, 60)
                Next
                _pieChart.Invalidate()

                For Each slc In _donutChart.Slices
                    slc.Value = _rand.Next(10, 60)
                Next
                _donutChart.Invalidate()

                For Each srs In _radarChart.Series
                    For i As Integer = 0 To srs.Values.Count - 1
                        srs.Values(i) = _rand.Next(40, 99)
                    Next
                Next
                _radarChart.Invalidate()

                _gaugeChart.Value = _rand.Next(20, 98)
                _bulletChart.Value = _rand.Next(40, 95)

                Dim sparkData(10) As Double
                For i As Integer = 0 To 10
                    sparkData(i) = _rand.Next(10, 65)
                Next
                _sparklineChart.SetData(sparkData)
            End Sub
        End Sub

        Private Function CreateCard(title As String, subtitle As String, chart As Control) As Panel
            Dim card As New Panel With {
                .Dock = DockStyle.Fill,
                .Margin = New Padding(6),
                .Padding = New Padding(14, 12, 14, 12),
                .BackColor = Color.White
            }
            AddHandler card.Paint, Sub(s, e)
                Using pen As New Pen(If(_isDark, Color.FromArgb(51, 65, 85), Color.FromArgb(226, 232, 240)), 1.0F)
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1)
                End Using
            End Sub

            Dim pnlHeader As New Panel With {
                .Dock = DockStyle.Top,
                .Height = 44,
                .BackColor = Color.Transparent
            }
            Dim lblTitle As New Label With {
                .Text = title,
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(15, 23, 42),
                .AutoSize = True,
                .Location = New Point(0, 0)
            }
            Dim lblSubtitle As New Label With {
                .Text = subtitle,
                .Font = New Font("Segoe UI", 8.5F),
                .ForeColor = Color.FromArgb(100, 116, 139),
                .AutoSize = True,
                .Location = New Point(0, 22)
            }
            pnlHeader.Controls.Add(lblTitle)
            pnlHeader.Controls.Add(lblSubtitle)

            chart.Dock = DockStyle.Fill
            card.Controls.Add(chart)
            card.Controls.Add(pnlHeader)
            pnlHeader.SendToBack()

            _cards.Add(New CardItem(card, lblTitle, lblSubtitle, chart))
            Return card
        End Function

        Private Function CreateGrid(rows As Integer) As TableLayoutPanel
            Dim grid As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .ColumnCount = 2,
                .RowCount = rows,
                .AutoScroll = True,
                .Padding = New Padding(4)
            }
            grid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
            grid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
            For i As Integer = 0 To rows - 1
                grid.RowStyles.Add(New RowStyle(SizeType.Absolute, 320.0F))
            Next
            Return grid
        End Function

        Private Function CreateToolbarButton(text As String, bg As Color) As Button
            Dim btn As New Button With {
                .Text = text,
                .BackColor = bg,
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Height = 34,
                .AutoSize = True,
                .Padding = New Padding(10, 4, 10, 4),
                .Margin = New Padding(0, 0, 8, 0),
                .Cursor = Cursors.Hand
            }
            btn.FlatAppearance.BorderSize = 0
            Return btn
        End Function

        Private Sub PopulateAllCharts()
            ' Tab 1: Bars & Columns
            _barChart.Categories.Add("Q1")
            _barChart.Categories.Add("Q2")
            _barChart.Categories.Add("Q3")
            _barChart.Categories.Add("Q4")
            _barChart.Series.Add(New LengkoBarSeries("Revenue", New Double() {120, 150, 180, 220}, Color.FromArgb(59, 130, 246)))
            _barChart.Series.Add(New LengkoBarSeries("Profit", New Double() {45, 60, 75, 95}, Color.FromArgb(16, 185, 129)))

            _hBarChart.AddItem("TypeScript", 88, Color.FromArgb(59, 130, 246))
            _hBarChart.AddItem("C# / .NET", 94, Color.FromArgb(139, 92, 246))
            _hBarChart.AddItem("Python", 82, Color.FromArgb(245, 158, 11))
            _hBarChart.AddItem("Go", 75, Color.FromArgb(6, 182, 212))
            _hBarChart.AddItem("Rust", 70, Color.FromArgb(239, 68, 68))

            Dim stackedBar = DirectCast(_cards(2).Chart, LengkoStackedBarChart)
            stackedBar.Categories.Add("2021")
            stackedBar.Categories.Add("2022")
            stackedBar.Categories.Add("2023")
            stackedBar.Categories.Add("2024")
            stackedBar.Series.Add(New LengkoBarSeries("Cloud", New Double() {40, 55, 75, 110}, Color.FromArgb(59, 130, 246)))
            stackedBar.Series.Add(New LengkoBarSeries("On-Prem", New Double() {60, 50, 45, 30}, Color.FromArgb(16, 185, 129)))
            stackedBar.Series.Add(New LengkoBarSeries("Support", New Double() {20, 25, 30, 35}, Color.FromArgb(245, 158, 11)))

            Dim hist = DirectCast(_cards(3).Chart, LengkoHistogramChart)
            hist.SetData(New Double() {18, 22, 25, 27, 30, 32, 34, 35, 38, 40, 42, 44, 45, 48, 52, 55, 60, 65, 72}, 6)

            Dim pareto = DirectCast(_cards(4).Chart, LengkoParetoChart)
            pareto.Items.Add(New LengkoParetoItem("Timeout 504", 142))
            pareto.Items.Add(New LengkoParetoItem("Auth 401", 95))
            pareto.Items.Add(New LengkoParetoItem("NullRef", 48))
            pareto.Items.Add(New LengkoParetoItem("DB Lock", 24))
            pareto.Items.Add(New LengkoParetoItem("Bad Format", 12))

            Dim waterfall = DirectCast(_cards(5).Chart, LengkoWaterfallChart)
            waterfall.Items.Add(New LengkoWaterfallItem("Start Cash", 120, isTotal:=True))
            waterfall.Items.Add(New LengkoWaterfallItem("Sales", 52))
            waterfall.Items.Add(New LengkoWaterfallItem("Subscriptions", 30))
            waterfall.Items.Add(New LengkoWaterfallItem("Salaries", -42))
            waterfall.Items.Add(New LengkoWaterfallItem("Cloud", -18))
            waterfall.Items.Add(New LengkoWaterfallItem("Marketing", -14))
            waterfall.Items.Add(New LengkoWaterfallItem("Net Cash", 0, isTotal:=True))

            ' Tab 2: Lines & Areas
            _lineChart.Series.Add(New LengkoLineSeries("Traffic (k)", New Double() {22, 34, 45, 40, 58, 65, 80}, Color.FromArgb(99, 102, 241)))
            _lineChart.Series.Add(New LengkoLineSeries("Signups", New Double() {10, 18, 24, 28, 35, 42, 55}, Color.FromArgb(236, 72, 153)))

            _areaChart.Labels.Add("Jan")
            _areaChart.Labels.Add("Feb")
            _areaChart.Labels.Add("Mar")
            _areaChart.Labels.Add("Apr")
            _areaChart.Labels.Add("May")
            _areaChart.Labels.Add("Jun")
            _areaChart.Series.Add(New LengkoAreaSeries("Inbound", New Double() {30, 45, 52, 68, 74, 90}, Color.FromArgb(59, 130, 246)))
            _areaChart.Series.Add(New LengkoAreaSeries("Outbound", New Double() {18, 25, 32, 40, 48, 62}, Color.FromArgb(16, 185, 129)))

            Dim stackedArea = DirectCast(_cards(8).Chart, LengkoStackedAreaChart)
            stackedArea.Categories.Add("Jan")
            stackedArea.Categories.Add("Feb")
            stackedArea.Categories.Add("Mar")
            stackedArea.Categories.Add("Apr")
            stackedArea.Categories.Add("May")
            stackedArea.Categories.Add("Jun")
            stackedArea.Series.Add(New LengkoAreaSeries("North", New Double() {25, 30, 38, 45, 52, 60}, Color.FromArgb(14, 165, 233)))
            stackedArea.Series.Add(New LengkoAreaSeries("South", New Double() {20, 24, 30, 36, 42, 48}, Color.FromArgb(168, 85, 247)))
            stackedArea.Series.Add(New LengkoAreaSeries("East", New Double() {15, 18, 22, 28, 32, 38}, Color.FromArgb(249, 115, 22)))

            Dim stepLine = DirectCast(_cards(9).Chart, LengkoStepLineChart)
            stepLine.Series.Add(New LengkoStepSeries("Inventory", New Double() {100, 85, 85, 60, 120, 95, 70}, Color.FromArgb(16, 185, 129)))
            stepLine.Series.Add(New LengkoStepSeries("Threshold", New Double() {50, 50, 50, 50, 50, 50, 50}, Color.FromArgb(239, 68, 68)))

            Dim combo = DirectCast(_cards(10).Chart, LengkoComboChart)
            combo.Categories.Add("Q1")
            combo.Categories.Add("Q2")
            combo.Categories.Add("Q3")
            combo.Categories.Add("Q4")
            combo.BarSeries.Add(New LengkoBarSeries("Units Sold", New Double() {320, 450, 580, 710}, Color.FromArgb(59, 130, 246)))
            combo.LineSeries.Add(New LengkoLineSeries("Margin %", New Double() {22, 28, 26, 31}, Color.FromArgb(239, 68, 68)))

            Dim generic = DirectCast(_cards(11).Chart, LengkoChart)
            generic.AddPoint("CPU", 48)
            generic.AddPoint("RAM", 72)
            generic.AddPoint("Disk", 60)
            generic.AddPoint("Network", 35)
            generic.AddPoint("GPU", 84)

            ' Tab 3: Circular & Polar
            _pieChart.Slices.Add(New LengkoPieSlice("North America", 38, Color.FromArgb(59, 130, 246)))
            _pieChart.Slices.Add(New LengkoPieSlice("Europe", 27, Color.FromArgb(16, 185, 129)))
            _pieChart.Slices.Add(New LengkoPieSlice("Asia Pacific", 23, Color.FromArgb(245, 158, 11)))
            _pieChart.Slices.Add(New LengkoPieSlice("Latin America", 12, Color.FromArgb(239, 68, 68)))

            _donutChart.Slices.Add(New LengkoPieSlice("Enterprise", 45, Color.FromArgb(99, 102, 241)))
            _donutChart.Slices.Add(New LengkoPieSlice("SMB", 30, Color.FromArgb(236, 72, 153)))
            _donutChart.Slices.Add(New LengkoPieSlice("Startup", 15, Color.FromArgb(20, 184, 166)))
            _donutChart.Slices.Add(New LengkoPieSlice("Consumer", 10, Color.FromArgb(245, 158, 11)))

            _radarChart.Axes.Add("Speed")
            _radarChart.Axes.Add("Reliability")
            _radarChart.Axes.Add("Usability")
            _radarChart.Axes.Add("Security")
            _radarChart.Axes.Add("Scalability")
            _radarChart.Series.Add(New LengkoRadarSeries("Lengko UI", New Double() {92, 95, 88, 90, 94}, Color.FromArgb(139, 92, 246)))
            _radarChart.Series.Add(New LengkoRadarSeries("Standard", New Double() {65, 70, 60, 75, 68}, Color.FromArgb(148, 163, 184)))

            Dim polar = DirectCast(_cards(15).Chart, LengkoPolarAreaChart)
            polar.Items.Add(New LengkoPolarItem("North", 45, Color.FromArgb(59, 130, 246)))
            polar.Items.Add(New LengkoPolarItem("NE", 60, Color.FromArgb(99, 102, 241)))
            polar.Items.Add(New LengkoPolarItem("East", 80, Color.FromArgb(139, 92, 246)))
            polar.Items.Add(New LengkoPolarItem("SE", 50, Color.FromArgb(236, 72, 153)))
            polar.Items.Add(New LengkoPolarItem("South", 35, Color.FromArgb(244, 63, 94)))
            polar.Items.Add(New LengkoPolarItem("SW", 55, Color.FromArgb(245, 158, 11)))
            polar.Items.Add(New LengkoPolarItem("West", 75, Color.FromArgb(16, 185, 129)))
            polar.Items.Add(New LengkoPolarItem("NW", 40, Color.FromArgb(20, 184, 166)))

            Dim sunburst = DirectCast(_cards(16).Chart, LengkoSunburstChart)
            sunburst.Items.Add(New LengkoSunburstItem("Software", 60, 0, Color.FromArgb(59, 130, 246)))
            sunburst.Items.Add(New LengkoSunburstItem("Hardware", 40, 0, Color.FromArgb(16, 185, 129)))
            sunburst.Items.Add(New LengkoSunburstItem("Desktop", 35, 1, Color.FromArgb(99, 102, 241)))
            sunburst.Items.Add(New LengkoSunburstItem("Mobile", 25, 1, Color.FromArgb(14, 165, 233)))
            sunburst.Items.Add(New LengkoSunburstItem("Servers", 25, 1, Color.FromArgb(34, 197, 94)))
            sunburst.Items.Add(New LengkoSunburstItem("IoT", 15, 1, Color.FromArgb(245, 158, 11)))

            ' Tab 4: Financial & Timelines
            Dim candle = DirectCast(_cards(17).Chart, LengkoCandlestickChart)
            candle.AddCandle("09:30", 150.2, 153.5, 149.8, 152.8, 142000)
            candle.AddCandle("10:00", 152.8, 154.0, 151.2, 151.6, 98000)
            candle.AddCandle("10:30", 151.6, 155.2, 151.0, 154.5, 175000)
            candle.AddCandle("11:00", 154.5, 156.0, 153.8, 155.8, 110000)
            candle.AddCandle("11:30", 155.8, 156.2, 153.0, 153.4, 130000)

            Dim ohlc = DirectCast(_cards(18).Chart, LengkoOhlcChart)
            ohlc.Bars.Add(New LengkoOhlcBar("Mon", 100, 108, 98, 105))
            ohlc.Bars.Add(New LengkoOhlcBar("Tue", 105, 110, 102, 104))
            ohlc.Bars.Add(New LengkoOhlcBar("Wed", 104, 114, 103, 112))
            ohlc.Bars.Add(New LengkoOhlcBar("Thu", 112, 115, 108, 109))
            ohlc.Bars.Add(New LengkoOhlcBar("Fri", 109, 118, 107, 116))

            Dim gantt = DirectCast(_cards(19).Chart, LengkoGanttChart)
            Dim baseDate = DateTime.Today
            gantt.Tasks.Add(New LengkoGanttItem("Architecture & API", baseDate, baseDate.AddDays(5), 100))
            gantt.Tasks.Add(New LengkoGanttItem("Core Chart Engine", baseDate.AddDays(3), baseDate.AddDays(10), 80))
            gantt.Tasks.Add(New LengkoGanttItem("32 WinForms Controls", baseDate.AddDays(6), baseDate.AddDays(15), 55))
            gantt.Tasks.Add(New LengkoGanttItem("Testing & Packaging", baseDate.AddDays(12), baseDate.AddDays(18), 20))

            ' Tab 5: Hierarchy & Flow
            Dim treemap = DirectCast(_cards(20).Chart, LengkoTreemap)
            treemap.Items.Add(New LengkoTreemapItem("Finance", 420))
            treemap.Items.Add(New LengkoTreemapItem("Engineering", 350))
            treemap.Items.Add(New LengkoTreemapItem("Marketing", 220))
            treemap.Items.Add(New LengkoTreemapItem("Support", 160))
            treemap.Items.Add(New LengkoTreemapItem("Legal & HR", 90))

            Dim sankey = DirectCast(_cards(21).Chart, LengkoSankeyChart)
            sankey.Links.Add(New LengkoSankeyLink("Organic Search", "Landing Page", 140))
            sankey.Links.Add(New LengkoSankeyLink("Paid Ads", "Landing Page", 90))
            sankey.Links.Add(New LengkoSankeyLink("Referral", "Product Specs", 65))
            sankey.Links.Add(New LengkoSankeyLink("Landing Page", "Pricing", 130))
            sankey.Links.Add(New LengkoSankeyLink("Landing Page", "Checkout", 70))
            sankey.Links.Add(New LengkoSankeyLink("Pricing", "Checkout", 100))
            sankey.Links.Add(New LengkoSankeyLink("Product Specs", "Checkout", 45))

            Dim funnel = DirectCast(_cards(22).Chart, LengkoFunnelChart)
            funnel.Stages.Add(New LengkoFunnelStage("Website Visits", 12500))
            funnel.Stages.Add(New LengkoFunnelStage("Sign Up", 5200))
            funnel.Stages.Add(New LengkoFunnelStage("Cart Added", 2800))
            funnel.Stages.Add(New LengkoFunnelStage("Checkout Started", 1600))
            funnel.Stages.Add(New LengkoFunnelStage("Purchased", 1100))

            Dim pyramid = DirectCast(_cards(23).Chart, LengkoPyramidChart)
            pyramid.Stages.Add(New LengkoPyramidStage("Executives", 8))
            pyramid.Stages.Add(New LengkoPyramidStage("Directors", 24))
            pyramid.Stages.Add(New LengkoPyramidStage("Team Leads", 65))
            pyramid.Stages.Add(New LengkoPyramidStage("Specialists", 180))
            pyramid.Stages.Add(New LengkoPyramidStage("Associates", 340))

            Dim heatmap = DirectCast(_cards(24).Chart, LengkoHeatmapChart)
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
            Dim matrix(,) As Double = {
                {20, 45, 78, 60, 30},
                {35, 60, 92, 85, 40},
                {40, 75, 95, 80, 50},
                {30, 65, 88, 70, 45},
                {25, 50, 70, 90, 65}
            }
            For r As Integer = 0 To 4
                For c As Integer = 0 To 4
                    heatmap.SetCell(r, c, matrix(r, c))
                Next
            Next

            ' Tab 6: KPIs & Statistical
            _gaugeChart.Value = 76.5
            _gaugeChart.Title = "Server Load"
            _gaugeChart.Unit = "%"

            _bulletChart.Title = "YTD Sales"
            _bulletChart.Subtitle = "$ in Thousands"
            _bulletChart.Value = 82
            _bulletChart.Target = 88
            _bulletChart.BadThreshold = 50
            _bulletChart.SatisfactoryThreshold = 75

            _sparklineChart.SetData(New Double() {14, 18, 15, 22, 28, 25, 32, 40, 38, 48, 52})

            Dim scatter = DirectCast(_cards(28).Chart, LengkoScatterChart)
            Dim blue = Color.FromArgb(59, 130, 246)
            scatter.Points.Add(New LengkoScatterPoint(10, 25, blue, "A1"))
            scatter.Points.Add(New LengkoScatterPoint(20, 38, blue, "A2"))
            scatter.Points.Add(New LengkoScatterPoint(30, 52, blue, "A3"))
            scatter.Points.Add(New LengkoScatterPoint(40, 66, blue, "A4"))
            scatter.Points.Add(New LengkoScatterPoint(50, 80, blue, "A5"))
            Dim red = Color.FromArgb(239, 68, 68)
            scatter.Points.Add(New LengkoScatterPoint(12, 18, red, "B1"))
            scatter.Points.Add(New LengkoScatterPoint(22, 30, red, "B2"))
            scatter.Points.Add(New LengkoScatterPoint(32, 40, red, "B3"))
            scatter.Points.Add(New LengkoScatterPoint(42, 58, red, "B4"))
            scatter.Points.Add(New LengkoScatterPoint(52, 70, red, "B5"))

            Dim bubble = DirectCast(_cards(29).Chart, LengkoBubbleChart)
            bubble.Points.Add(New LengkoBubblePoint(15, 45, 14, "Opportunity A", Color.FromArgb(59, 130, 246)))
            bubble.Points.Add(New LengkoBubblePoint(30, 70, 22, "Opportunity B", Color.FromArgb(16, 185, 129)))
            bubble.Points.Add(New LengkoBubblePoint(55, 35, 18, "Opportunity C", Color.FromArgb(245, 158, 11)))
            bubble.Points.Add(New LengkoBubblePoint(75, 85, 30, "Strategic Bet", Color.FromArgb(139, 92, 246)))
            bubble.Points.Add(New LengkoBubblePoint(85, 50, 16, "Core Business", Color.FromArgb(236, 72, 153)))

            Dim boxplot = DirectCast(_cards(30).Chart, LengkoBoxPlotChart)
            boxplot.Items.Add(New LengkoBoxPlotItem("Group A", 20, 35, 50, 68, 85))
            boxplot.Items.Add(New LengkoBoxPlotItem("Group B", 30, 45, 62, 78, 92))
            boxplot.Items.Add(New LengkoBoxPlotItem("Group C", 15, 28, 42, 58, 75))

            Dim errorbar = DirectCast(_cards(31).Chart, LengkoErrorBarChart)
            errorbar.Points.Add(New LengkoErrorPoint("Sample 1", 45, 5, 6))
            errorbar.Points.Add(New LengkoErrorPoint("Sample 2", 62, 8, 7))
            errorbar.Points.Add(New LengkoErrorPoint("Sample 3", 53, 4, 5))
            errorbar.Points.Add(New LengkoErrorPoint("Sample 4", 78, 9, 8))
        End Sub

    End Class

End Namespace
