using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BicycleVectors
{
    public partial class Form1 : Form
    {
        public double BikeSpeed;
        public double BikeDirection;
        public double WindSpeed;
        public double WindDirection;
        PointF[] graphicPoint;
        double BikeXDir;
        double BikeYDir;
        double WindXDir;
        double WindYDir;
        Rectangle formBounds;

        public Form1()
        {
            InitializeComponent();
            graphicPoint = new PointF[1] { new PointF(0f, 0f) };
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        { 
            BikeSpeed = double.Parse(txtBikeSpeed.Text);
            BikeDirection = double.Parse(txtBikeDirection.Text);
            WindSpeed = double.Parse(txtWindSpeed.Text);
            WindDirection = double.Parse(txtWindDirection.Text);
            BikeDirection -= 90;
            WindDirection -= 90;
            BikeXDir = ((BikeSpeed * 0.3) * Math.Cos(ConvertDegreesToRadians(BikeDirection)));
            BikeYDir = ((BikeSpeed * 0.3) * Math.Sin(ConvertDegreesToRadians(BikeDirection)));
            WindXDir = ((WindSpeed * 0.5) * Math.Cos(ConvertDegreesToRadians(WindDirection)));
            WindYDir = ((WindSpeed * 0.5) * Math.Sin(ConvertDegreesToRadians(WindDirection)));
            BikeXDir = ElimateExtremeValues(BikeXDir);
            BikeYDir = ElimateExtremeValues(BikeYDir);
            WindXDir = ElimateExtremeValues(WindXDir);
            WindYDir = ElimateExtremeValues(WindYDir);



            (double BikeR, double BikeTheta) = ConvertPolarToRectangular(BikeSpeed, BikeDirection);
            (double WindR, double WindTheta) = ConvertPolarToRectangular(WindSpeed, WindDirection);
            double Speed = BikeR + WindR;
            double Direction = BikeTheta + WindTheta;             
            (BikeSpeed, BikeDirection) = ConvertRectangularToPolar(Speed, Direction);
            label6.Text = "";
            label6.Text += string.Format($"bike X {BikeXDir} bike Y  {BikeYDir} \r\n");
            label6.Text += string.Format($"wind X {WindXDir} wind Y {WindYDir} \r\n");
            this.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtBikeSpeed.Text = "500";
            txtBikeDirection.Text = "0";
            txtWindSpeed.Text = "100";
            txtWindDirection.Text = "90";
            BikeXDir = ((BikeSpeed * 0.3) * Math.Cos(BikeDirection));
            BikeYDir = ((BikeSpeed * 0.3) * Math.Sin(BikeDirection));
            WindXDir = ((WindSpeed * 0.5) * Math.Cos(WindDirection));
            WindYDir = ((WindSpeed * 0.5) * Math.Sin(WindDirection));
            label6.Text = "";
            label6.Text = string.Format($"bike X {BikeXDir} bike Y {BikeYDir} \r\n");
            label6.Text += string.Format($"wind X {WindXDir} wind Y {WindYDir} \r\n");
            this.Refresh();
          
            formBounds = this.Bounds;
            formBounds.Offset(x: 100, y: 50);




        }

        public (double R, double Theta) ConvertRectangularToPolar(double x, double y)
        {          
            double R = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double Theta = Math.Atan2(y, x);
            double ThetaDegrees = ConvertRadiansToDegrees(Theta);
            
            if (ThetaDegrees <= -1)
                ThetaDegrees = 360 - ThetaDegrees;
            return (R, ThetaDegrees);
        }

        public (double x, double y) ConvertPolarToRectangular(double r, double Theta)
        {
            double RadianTheta = ConvertDegreesToRadians(Theta);         
            double x = r * Math.Cos(RadianTheta);
            double y = r * Math.Sin(RadianTheta);
            
            return (x, y);
        }

        public double AdjustAngleFromQuadrant(double result, double xVar, double yVar)
        {
            if (xVar < 0 && yVar < 0)
            {
                result = (180 / Math.PI) * result;
                result = result + 180;
            }
            else if (yVar < 0)
            {
                result = (180 / Math.PI) * result;
                result = 360 - result;
            }
            else if (xVar < 0 && yVar == 0)
            {
                result = 180;
            }
            else
            {
                result = (180 / Math.PI) * result;
            }
            return result;
        }

        public double ConvertDegreesToRadians(double val)
        {
            val = (Math.PI / 180 * val);
            return val;
        }

        public double ConvertRadiansToDegrees(double val)
        {
            return (180 / Math.PI * val);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            graphicPoint[0].X = e.X;
            graphicPoint[0].Y = e.Y;
            this.Refresh();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            //e.Graphics.PageUnit = GraphicsUnit.Pixel;
            //e.Graphics.ScaleTransform(1.36f, -1.27f);
            //e.Graphics.TranslateTransform(800f, 419f);

            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            e.Graphics.ScaleTransform(3.2f, -1.674f);
            e.Graphics.TranslateTransform(350f, -250f);

            if (graphicPoint != null) ;
            e.Graphics.TransformPoints(System.Drawing.Drawing2D.CoordinateSpace.World, System.Drawing.Drawing2D.CoordinateSpace.Page, graphicPoint);
            label5.Text = graphicPoint[0].X + " " + graphicPoint[0].Y;
            float bikex = float.Parse(BikeXDir.ToString());
            float bikey = float.Parse(BikeYDir.ToString());
            float windx = float.Parse(WindXDir.ToString());
            float windy = float.Parse(WindYDir.ToString());
           
            
            PointF BikeStart = new PointF(0F, 0f);
            PointF BikeEnd = new PointF(bikex, bikey);
            PointF WindStart = new PointF(0f, 0f);
            PointF WindEnd = new PointF(windx, windy);
            Pen BluePen = new Pen(Color.Blue, 10f);
            Pen RedPen = new Pen(Color.Red, 10f);
            //BikeEnd.Y = -BikeEnd.Y;
            //WindEnd.Y = -WindEnd.Y;
           
            BluePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            RedPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            e.Graphics.DrawLine(BluePen, BikeStart, BikeEnd);
            e.Graphics.DrawLine(RedPen, WindStart, WindEnd);
            e.Graphics.DrawLine(BluePen, BikeStart, BikeEnd);
            e.Graphics.DrawLine(RedPen, WindStart, WindEnd);
            PointF Point = new PointF(0f, 0f);
            SizeF Size = new SizeF(10f, 10f);
            RectangleF arcRec = new RectangleF(Point, Size);
            BluePen.Color = Color.PeachPuff;
            e.Graphics.DrawArc(BluePen, arcRec, 0, 360);
        }

        private double ElimateExtremeValues(double value)
        {
            double intPart = Math.Truncate(value);
            double fracPart = value - intPart;
            if (fracPart < 0.01)
            {
                fracPart = 0;
               value = intPart + fracPart;
            }

            return value;
        }

    }
}
