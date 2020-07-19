using System;
using System.Drawing;
using System.Reflection;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraSplashScreen;

namespace Cube.Win
{
    public partial class XafSplashScreen : SplashScreen
    {
        private void LoadBlankLogo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            labelSubtitle.Text = $"Cоздание заказов. Версия {assembly.GetName().Version}";
        }
        protected override void DrawContent(GraphicsCache graphicsCache, Skin skin)
        {
            Rectangle bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            graphicsCache.Graphics.DrawRectangle(graphicsCache.GetPen(Color.FromArgb(255, 87, 87, 87), 1), bounds);
        }
        protected void UpdateLabelsPosition()
        {
            labelApplicationName.CalcBestSize();
            int newLeft = (Width - labelApplicationName.Width) / 2;
            labelApplicationName.Location = new Point(newLeft, labelApplicationName.Top);
            labelSubtitle.CalcBestSize();
            newLeft = (Width - labelSubtitle.Width) / 2;
            labelSubtitle.Location = new Point(newLeft, labelSubtitle.Top);
        }
        public XafSplashScreen()
        {
            InitializeComponent();
            LoadBlankLogo();
            this.labelCopyright.Text = "Copyright © " + DateTime.Now.Year + "МФ Куб" + System.Environment.NewLine + "Все права защищены.";
            UpdateLabelsPosition();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            if ((UpdateSplashCommand)cmd == UpdateSplashCommand.Description)
            {
                labelStatus.Text = (string)arg;
            }
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}