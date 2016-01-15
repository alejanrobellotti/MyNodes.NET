﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MyNetSensors.LogicalNodes;
using MyNetSensors.LogicalNodesUI;
using MyNetSensors.SerialControllers;

namespace MyNetSensors.WebController.Controllers
{
    public class DashboardController:Controller
    {

        private LogicalNodesUIEngine engineUI = SerialController.logicalNodesUIEngine;


        public IActionResult Index()
        {
            return View();
        }


        public List<LogicalNodePanel> GetPanels()
        {
            if (engineUI == null)
                return null;

            return engineUI.GetPanels();
        }

        public string GetNameForPanel(string id)
        {
            if (engineUI == null)
                return null;

            LogicalNodePanel panel = engineUI.GetPanel(id);
            if (panel == null)
                return null;
            else return panel.Name;
        }

        public List<LogicalNodeUI> GetUINodes()
        {
            if (engineUI == null)
                return null;

            return engineUI.GetUINodes();
        }


        public bool ClearLog(string nodeId)
        {
            engineUI.ClearLog(nodeId);
            return true;
        }


        public bool TextBoxSend(string nodeId,string value)
        {
            engineUI.TextBoxSend(nodeId,value);
            return true;
        }

        

        public bool ButtonClick(string nodeId)
        {
            engineUI.ButtonClick(nodeId);
            return true;
        }

        public bool ToggleButtonClick(string nodeId)
        {
            engineUI.ToggleButtonClick(nodeId);
            return true;
        }

        public bool SwitchClick(string nodeId)
        {
            engineUI.SwitchClick(nodeId);
            return true;
        }

        public bool SliderChange(string nodeId, int value)
        {
            engineUI.SliderChange(nodeId,value);
            return true;
        }

        public bool RGBSlidersChange(string nodeId, string value)
        {
            engineUI.RGBSlidersChange(nodeId, value);
            return true;
        }

        public bool RGBWSlidersChange(string nodeId, string value)
        {
            engineUI.RGBWSlidersChange(nodeId, value);
            return true;
        }

        
    }
}
