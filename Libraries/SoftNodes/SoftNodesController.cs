﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyNetSensors.Gateway;

namespace MyNetSensors.SoftNodes
{
    public class SoftNodesController
    {
        private ISoftNodesServer server;
        private SerialGateway gateway;

        public SoftNodesController(ISoftNodesServer server, SerialGateway gateway)
        {
            this.server = server;
            this.gateway = gateway;
            server.OnReceivedMessageEvent+= OnReceiverSoftSerialMessage;
            gateway.OnMessageSendEvent+= OnSendGatewayMessage;
        }

        private void OnSendGatewayMessage(Message message)
        {
            server.SendMessage(message);
        }

        private void OnReceiverSoftSerialMessage(Message message)
        {
            gateway.RecieveMessage(message);
        }

    }
}
