using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public class Screens
    {
		public static List<ScreenModel> GetScreenInfo()
		{
			List<ScreenModel> screenList = new List<ScreenModel>();
			
			int monId = 1;
			foreach ( Screen screen in Screen.AllScreens)
			{
				ScreenModel model = new ScreenModel();
				model.ScreenNumber = monId;
				model.ScreenWidth = screen.Bounds.Width;
				model.ScreenHeight = screen.Bounds.Height;
				model.Xpos = screen.Bounds.X;
				model.Ypos = screen.Bounds.Y;

				screenList.Add(model);

				model = null;
				monId++;
			}

			return screenList;
		}
    }
}
