using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comp
{
    public partial class ScaleComp : UserControl
    {
        private string unit;   //Единицы измерения 
        private int imin = 0;   //Минимальное значение шкалы
        //private double fmin = 0.0;
       // private double fmax = 0.0;
        private int imax = 10;   //Максимальное значение
        private int istep = 1;   //шаг
        private int current_data = 0;   //Текущее значение
        private Color scale = Color.DarkRed;   //Цвет шкалы
        private Color arrow = Color.Red;   //Цвет стрелки
        private Color bg_scale = Color.Red;   //Цвет круга шкалы
        private float radius = 0.0f;   //Радиус
        private float ratio = 0.0f;   //Коэфф для вычисления
        private int rotation = 180;   //Коэфф для вычисления
        private float current_ratio = 0f;   //Текущее значение угла
        private int data_on_scale = 0;   //Текущее значение шкалы
        //private string min_str = "0";
       // private string max_str = "10";
       // private bool float_flag = false;
        public Color LowerColor
        {
            get
            {
                return panel1.BackColor;
            }
            set
            {
                panel1.BackColor = value;
            }
        }

        public Color UpperColor
        {
            get
            {
                return panel2.BackColor;
            }
            set
            {
                panel2.BackColor = value;

                label1.BackColor = panel2.BackColor;
                label2.BackColor = panel2.BackColor;
            }
        }

        public Color TextPanelColor
        {
            get
            {
                return richTextBox1.BackColor;
            }
            set
            {
                richTextBox1.BackColor = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return richTextBox1.ForeColor;
            }
            set
            {
                richTextBox1.ForeColor = value;
            }
        }

        public string Measurement
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }

    /*    public string MinScale
        {
            get
            {
                return min_str;
            }
            set
            {
                min_str = value;
                if (!float_flag)
                {
                    if (int.TryParse(min_str, out imin))
                        float_flag = true;
                    else fmin = Convert.ToDouble(min_str);
                }
                else fmin = Convert.ToDouble(min_str);
                label1.Text = min_str;
            }
        }
        */
        public int MaxScale
        {
            get
            {
                return imax;
            }
            set
            {
                imax = value;
                label2.Text =Convert.ToString(imax);
            }
        }

        public int MinScale
        {
            get
            {
                return imin;
            }
            set
            {
                imin = value;
                label1.Text = Convert.ToString(imin);
            }
        }
        /* public string MaxScale
          {
              get
              {
                  return max_str;
              }
              set
              {
                  max_str = value;
                  if (!float_flag)
                  {
                      if (int.TryParse(max_str, out imax))
                          float_flag = true;
                      else fmax = Convert.ToDouble(max_str);
                  }
                  else fmax = Convert.ToDouble(max_str);
                  label2.Text = max_str;
              }
          }*/

        public int Step
        {
            get
            {
                return istep;
            }
            set
            {
                istep = value;
            }
        }

        public Color ScaleColor
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public Color ArrowColor
        {
            get
            {
                return arrow;
            }
            set
            {
                arrow = value;
            }
        }

        public Color BackGroundScaleColor
        {
            get
            {
                return bg_scale;
            }
            set
            {
                bg_scale = value;
            }

        }

        public ScaleComp()
        {
            InitializeComponent();

            timer1 = new Timer();
            timer1.Interval = 500;
            timer1.Tick += new EventHandler(Timer1_Tick);
            richTextBox1.Text = Convert.ToString(current_data) + " " + unit;
            label1.Text = Convert.ToString(imin);
            label2.Text = Convert.ToString(imax);
            label1.Location = new System.Drawing.Point(label1.Width - 10, panel2.Height - 15);
            label2.Location = new System.Drawing.Point(panel2.Width - label2.Width, panel2.Height - 15);
        }
        //Очистка панели
        private void draw_panel_clear(Color uppercolor)
        {
            using (Graphics g = panel2.CreateGraphics())
            {
                g.Clear(uppercolor);
            }
        }

        //Рисование шкалы
        private void draw_panel_Paint()
        {
            using (Graphics g = panel2.CreateGraphics())
            {

                int i = 180;
               
                radius = (panel2.Width - 40) / 2;
                ratio = 180 / ((imax - imin) / istep);
                
               

                Pen pen_color = new Pen(scale, 2);
                Pen pen_color_bg = new Pen(bg_scale, 1);

                g.DrawLine((pen_color), 30, panel2.Height - 10, 20, panel2.Height - 10);
                g.DrawLine((pen_color), panel2.Width - 20, panel2.Height - 10, panel2.Width - 30, panel2.Height - 10);

                g.DrawRectangle(pen_color, (int)radius + 20, panel2.Height - 10 - (int)radius, 1, 1);

                int x = 20;
                int y = panel2.Height - 10 - (int)radius;
                int width = 2 * (int)radius;
                int height = 2 * (int)radius;

                // Create start and sweep angles on ellipse.
                int startAngle = 180;
                int sweepAngle = 180;

                // Draw arc to screen.
                g.DrawArc(pen_color_bg, x, y, width, height, startAngle, sweepAngle);

                //Рисование точек
                for (i = 180 + (int)ratio; i < 360; i += (int)ratio)
                {
                    double rad = (double)i / 180 * 3.14;
                    double _x = radius * Math.Cos(rad) + 20 + radius;
                    double _y = radius * Math.Sin(rad) + panel2.Height - 10;

                    double __x = (radius - 5) * Math.Cos(rad) + 20 + radius;
                    double __y = (radius - 5) * Math.Sin(rad) + panel2.Height - 10;

                     g.DrawLine((pen_color), (float)_x, (float)_y, (float)__x, (float)__y);
                    //g.DrawRectangle(pen_color, (int)_x, (int)_y, 1, 1);
                }
            }
        }

        //Добавление шкалы и обновление лейблов
        private void draw_scale()
        {
            draw_panel_clear(panel2.BackColor);
            label1.BackColor = panel2.BackColor;
            label2.BackColor = panel2.BackColor;
            draw_panel_Paint();
        }

        private int draw_arrow(int min, int current_rotation, int cur_data, float rat)
        {
            if (cur_data >= data_on_scale) //cur_data -поступившее значение  
            {
                using (Graphics g = panel2.CreateGraphics())
                {
                    Pen arrow_color = new Pen(arrow, 2);
                    Pen bg_color = new Pen(panel2.BackColor, 2);

                    double rad = (double)current_rotation / 180 * 3.14;
                    double prev_rad = ((double)current_rotation- current_ratio) / 180 * 3.14;

                    double __x = (radius - 10) * Math.Cos(rad) + 20 + radius;
                    double __y = (radius - 10) * Math.Sin(rad) + panel2.Height - 10;
               
                    double _x = (radius - 10) * Math.Cos(prev_rad) + 20 + radius;
                    double _y = (radius - 10) * Math.Sin(prev_rad) + panel2.Height - 10;
                    

                    g.DrawLine(bg_color, (int)_x, (int)_y, 20 + radius, panel2.Height - 10);
                    g.DrawLine(arrow_color, (int)__x, (int)__y, 20 + radius, panel2.Height - 10);

                    richTextBox1.Text = Convert.ToString(data_on_scale) + " " + unit;

                    data_on_scale ++;                    
                }
            }
            return (current_rotation + (int)current_ratio);
        }
        //Событие на тике 
        private void Timer1_Tick(object Sender, EventArgs e)
        {
            rotation = draw_arrow(imin, rotation, current_data, current_ratio);
        }

        //Процедура для старта отображения 
        public void GetData(int data)
        {
            rotation = 180;
            data_on_scale = imin;
            current_data = data;
            draw_scale();
            current_ratio = 180 / (imax - imin);
       
            timer1.Enabled = true;

        }   
    }
}
