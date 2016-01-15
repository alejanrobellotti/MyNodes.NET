﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MyNetSensors.LogicalNodes;

namespace MyNetSensors.LogicalNodesUI
{
    public delegate void LogicalNodeUIEventHandler(LogicalNodeUI node);

    public class LogicalNodesUIEngine
    {
        public event LogicalNodeUIEventHandler OnNewUINodeEvent;
        public event LogicalNodeUIEventHandler OnRemoveUINodeEvent;
        public event LogicalNodeUIEventHandler OnUINodeUpdatedEvent;

        private static LogicalNodesEngine engine;

        public LogicalNodesUIEngine(LogicalNodesEngine engine)
        {
            LogicalNodesUIEngine.engine = engine;
            engine.OnNewNodeEvent += OnNewNodeEvent;
            engine.OnRemoveNodeEvent += OnRemoveNodeEvent;
            engine.OnNodeUpdatedEvent += OnNodeUpdatedEvent;
            engine.OnOutputUpdatedEvent += OnOutputUpdatedEvent;
            engine.OnInputUpdatedEvent += OnInputUpdatedEvent;
            engine.OnNewLinkEvent += OnNewLinkEvent;
            engine.OnRemoveLinkEvent += OnRemoveLinkEvent;
        }


        private void OnInputUpdatedEvent(Input input)
        {
            LogicalNode node = engine.GetInputOwner(input);
            if (node is LogicalNodeUI)
                OnUINodeUpdatedEvent?.Invoke((LogicalNodeUI)node);
        }

        private void OnOutputUpdatedEvent(Output output)
        {
            LogicalNode node = engine.GetOutputOwner(output);
            if (node is LogicalNodeUI)
                OnUINodeUpdatedEvent?.Invoke((LogicalNodeUI)node);
        }

        private void OnNodeUpdatedEvent(LogicalNode node)
        {
            if (node is LogicalNodeUI)
                OnUINodeUpdatedEvent?.Invoke((LogicalNodeUI)node);
        }

        private void OnRemoveNodeEvent(LogicalNode node)
        {
            if (node is LogicalNodeUI)
                OnRemoveUINodeEvent?.Invoke((LogicalNodeUI)node);
        }

        private void OnNewNodeEvent(LogicalNode node)
        {
            if (!(node is LogicalNodeUI)) return;

            LogicalNodeUI n = (LogicalNodeUI)node;

            n.Name = GenerateName(n);
            engine.UpdateNode(n);

            OnNewUINodeEvent?.Invoke(n);
        }

        private string GenerateName(LogicalNodeUI node)
        {
            //auto naming
            List<LogicalNodeUI> nodes = GetUINodesForPanel(node.PanelId);
            List<string> names = nodes.Select(x => x.Name).ToList();
            for (int i = 1; i <= names.Count + 1; i++)
            {
                if (!names.Contains($"{node.Name} {i}"))
                    return $"{node.Name} {i}";
            }
            return null;
        }


        private void OnNewLinkEvent(LogicalLink link)
        {
            LogicalNode outNode = engine.GetOutputOwner(link.OutputId);
            LogicalNode inNode = engine.GetInputOwner(link.InputId);

            Output output = engine.GetOutput(link.OutputId);
            Input input = engine.GetInput(link.InputId);
        }

        private void OnRemoveLinkEvent(LogicalLink link)
        {
            LogicalNode inNode = engine.GetInputOwner(link.InputId);
            LogicalNode outNode = engine.GetOutputOwner(link.OutputId);
        }

        public List<LogicalNodePanel> GetPanels()
        {
            return engine.nodes
                .Where(n => n is LogicalNodePanel)
                .Cast<LogicalNodePanel>()
                .ToList();
        }

        public LogicalNodePanel GetPanel(string id)
        {
            LogicalNode node = engine.GetNode(id);

            return node as LogicalNodePanel;
        }


        public List<LogicalNodeUI> GetUINodes()
        {
            return engine.nodes
                .Where(n => n is LogicalNodeUI)
                .Cast<LogicalNodeUI>()
                .ToList();
        }

        public List<LogicalNodeUI> GetUINodesForPanel(string panelId)
        {
            return engine.nodes
                .Where(n => n is LogicalNodeUI && n.PanelId==panelId)
                .Cast<LogicalNodeUI>()
                .ToList();
        }


        public void ClearLog(string nodeId)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUILog))
                return;

            LogicalNodeUILog node = (LogicalNodeUILog)n;
            node.ClearLog();

            //send update ivent
            engine.UpdateNode(node);
        }


        public void ButtonClick(string nodeId)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUIButton))
                return;

            LogicalNodeUIButton node = (LogicalNodeUIButton)n;
            node.Click();
        }



        public void ToggleButtonClick(string nodeId)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUIToggleButton))
                return;

            LogicalNodeUIToggleButton node = (LogicalNodeUIToggleButton)n;
            node.Toggle();
        }

        public void SwitchClick(string nodeId)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUISwitch))
                return;

            LogicalNodeUISwitch node = (LogicalNodeUISwitch)n;
            node.Toggle();
        }

        public void SliderChange(string nodeId, int value)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUISlider))
                return;

            LogicalNodeUISlider node = (LogicalNodeUISlider)n;
            node.SetValue(value);
        }


        public void RGBSlidersChange(string nodeId, string value)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUIRGBSliders))
                return;

            LogicalNodeUIRGBSliders node = (LogicalNodeUIRGBSliders)n;
            node.SetValue(value);
        }


        public void RGBWSlidersChange(string nodeId, string value)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUIRGBWSliders))
                return;

            LogicalNodeUIRGBWSliders node = (LogicalNodeUIRGBWSliders)n;
            node.SetValue(value);
        }





        public void TextBoxSend(string nodeId, string value)
        {
            LogicalNode n = engine.GetNode(nodeId);
            if (!(n is LogicalNodeUITextBox))
                return;

            LogicalNodeUITextBox node = (LogicalNodeUITextBox)n;
            node.Send(value);
        }


    }
}
