using System;
using System.IO;
using SkiaSharp;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bronzetti.christian._4h.gatto2
{
    

    public partial class MainPage : ContentPage
    {
        SKPath gatto = new SKPath();
        List<SKPoint> disegno = new List<SKPoint>();
        public MainPage()
        {
            InitializeComponent();
        }

      
        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;

            //tela
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            //disegno
            SKPath pathDisegno = new SKPath();

            SKPoint p = new SKPoint(0, 0);
            pathDisegno.MoveTo(p); //non lascia traccia

            foreach(SKPoint punto in disegno)
                pathDisegno.LineTo(punto);

            //pennello
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 1,
            };

            //pennello
            SKPaint paintDisegno = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Green,
                StrokeWidth = 2,
            };

            canvas.DrawPath(gatto, paint);
            canvas.DrawPath(pathDisegno, paintDisegno);

        }

        private async void btnCarica_Clicked(object sender, EventArgs e)
        {
        //documentazione
        https://docs.microsoft.com/it-it/xamarin/essentials/file-system-helpers?tabs=ios

            //cache
            var cacheDir = FileSystem.CacheDirectory;

            //app data
            var mainDir = FileSystem.AppDataDirectory;

            //packages : file che alleghiamo al nostro eseguibile
            //per uwp aggiungere i file nlla radice del progetto e contrassegnare l'azione di compilaizone come content per poi aprirla

            var stream = await FileSystem.OpenAppPackageFileAsync("Gatto.csv");
            var reader = new StreamReader(stream);

            while(!reader.EndOfStream)
            {
                string str = reader.ReadLine();
                string[] colonne = str.Split(',');
                float X, Y;
                float.TryParse(colonne[1], out X);
                float.TryParse(colonne[2], out Y);

                SKPoint p = new SKPoint(X, Y);

                if(colonne[0] == "L")
                    gatto.LineTo(p);
                
                else if(colonne[0] == "M")
                    gatto.MoveTo(p);
                
            }

            telaDisegno.InvalidateSurface();
        }

        private async void btnCaricaDisegno_Clicked(object sender, EventArgs e)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("quadrato.dis");
            var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();
                string[] colonne = str.Split(',');

                float X, Y;
                float.TryParse(colonne[0], out X);
                float.TryParse(colonne[1], out Y);

                SKPoint p = new SKPoint(X, Y);
                disegno.Add(p);
            }
             telaDisegno.InvalidateSurface();
            
        }
    }
}
