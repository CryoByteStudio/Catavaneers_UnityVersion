using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catavaneer.MenuSystem
{
    public class LoseMenu : Menu<LoseMenu>
    {
        #region PUBLIC METHODS
        public void OnRestartPressed()
        {
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            MenuManager.LoadMainMenuLevel();
        }
        #endregion
    }
}