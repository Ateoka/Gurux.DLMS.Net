//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// More information of Gurux products: http://www.gurux.org
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Internal;

namespace Gurux.DLMS.Objects
{
    public class GXDLMSAutoAnswer : GXDLMSObject, IGXDLMSBase
    {
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSAutoAnswer()
            : base(ObjectType.AutoAnswer, "0.0.2.2.0.255", 0)
        {
            ListeningWindow = new List<KeyValuePair<GXDateTime, GXDateTime>>();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logican Name of the object.</param>
        public GXDLMSAutoAnswer(string ln)
            : base(ObjectType.AutoAnswer, ln, 0)
        {
            ListeningWindow = new List<KeyValuePair<GXDateTime, GXDateTime>>();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logican Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSAutoAnswer(string ln, ushort sn)
            : base(ObjectType.AutoAnswer, ln, sn)
        {
            ListeningWindow = new List<KeyValuePair<GXDateTime, GXDateTime>>();
        }

        [XmlIgnore()]
        public AutoConnectMode Mode
        {
            get;
            set;
        }

        [XmlIgnore()]
        public List<KeyValuePair<GXDateTime, GXDateTime>> ListeningWindow
        {
            get;
            set;
        }

        [XmlIgnore()]
        public AutoAnswerStatus Status
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int NumberOfCalls
        {
            get;
            set;
        }
        
        /// <summary>
        /// Number of rings within the window defined by ListeningWindow.        
        /// </summary>
        [XmlIgnore()]
        public int NumberOfRingsInListeningWindow
        {
            get;
            set;
        }

        /// <summary>
        /// Number of rings outside the window defined by ListeningWindow.        
        /// </summary>
        [XmlIgnore()]
        public int NumberOfRingsOutListeningWindow
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, Mode, ListeningWindow, Status, 
                NumberOfCalls, NumberOfRingsInListeningWindow + "/" + NumberOfRingsOutListeningWindow};
        }

        #region IGXDLMSBase Members

        int[] IGXDLMSBase.GetAttributeIndexToRead()
        {
            List<int> attributes = new List<int>();
            //LN is static and read only once.
            if (string.IsNullOrEmpty(LogicalName))
            {
                attributes.Add(1);
            }
            //Mode is static and read only once.
            if (!base.IsRead(2))
            {
                attributes.Add(2);
            }
            //ListeningWindow is static and read only once.
            if (!base.IsRead(3))
            {
                attributes.Add(3);
            }
            //Status is not static.
            if (CanRead(4))
            {
                attributes.Add(4);
            }
            
            //NumberOfCalls is static and read only once.
            if (!base.IsRead(5))
            {
                attributes.Add(5);
            }
            //NumberOfRingsInListeningWindow is static and read only once.
            if (!base.IsRead(6))
            {
                attributes.Add(6);
            }
            return attributes.ToArray();
        }

        int IGXDLMSBase.GetAttributeCount()
        {
            return 6;
        }

        int IGXDLMSBase.GetMethodCount()
        {
            return 0;
        }

        override public DataType GetDataType(int index)
        {
            if (index == 1)
            {
                return DataType.OctetString;
            }
            if (index == 2)
            {
                return DataType.Enum;
            }
            if (index == 3)
            {
                return DataType.Array;
            }
            if (index == 4)
            {
                return DataType.Enum;
            }
            if (index == 5)
            {
                return DataType.UInt8;
            }
            if (index == 6)
            {
                return DataType.Array;
            }
            throw new ArgumentException("GetDataType failed. Invalid attribute index.");
        }

        object IGXDLMSBase.GetValue(int index, int selector, object parameters)
        {
            if (index == 1)
            {
                return this.LogicalName;
            }
            if (index == 2)
            {
                return (byte)Mode;
            }
            if (index == 3)
            {
                int cnt = ListeningWindow.Count;
                List<byte> data = new List<byte>();
                data.Add((byte)DataType.Array);
                //Add count            
                GXCommon.SetObjectCount(cnt, data);
                if (cnt != 0)
                {
                    foreach (var it in ListeningWindow)
                    {
                        data.Add((byte)DataType.Structure);
                        data.Add((byte)2); //Count
                        GXCommon.SetData(data, DataType.OctetString, it.Key); //start_time
                        GXCommon.SetData(data, DataType.OctetString, it.Value); //end_time
                    }
                }
                return data.ToArray();                
            }
            if (index == 4)
            {
                return Status;
            }
            if (index == 5)
            {
                return NumberOfCalls;
            }
            if (index == 6)
            {
                List<byte> data = new List<byte>();
                data.Add((byte)DataType.Structure);
                //Add count            
                GXCommon.SetObjectCount(2, data);
                GXCommon.SetData(data, DataType.UInt8, NumberOfRingsInListeningWindow);
                GXCommon.SetData(data, DataType.UInt8, NumberOfRingsOutListeningWindow);                
                return data.ToArray();                
            }
            throw new ArgumentException("GetValue failed. Invalid attribute index.");
        }

        void IGXDLMSBase.SetValue(int index, object value)
        {
            if (index == 1)
            {
                if (value is string)
                {
                    LogicalName = value.ToString();
                }
                else
                {
                    LogicalName = GXDLMSClient.ChangeType((byte[])value, DataType.OctetString).ToString();
                }
            }
            else if (index == 2)
            {
                Mode = (AutoConnectMode)Convert.ToInt32(value);
            }
            else if (index == 3)
            {
                ListeningWindow.Clear();
                if (value != null)
                {
                    foreach (Object[] item in (Object[])value)
                    {
                        GXDateTime start = (GXDateTime)GXDLMSClient.ChangeType((byte[])item[0], DataType.DateTime);
                        GXDateTime end = (GXDateTime)GXDLMSClient.ChangeType((byte[])item[1], DataType.DateTime);
                        ListeningWindow.Add(new KeyValuePair<GXDateTime, GXDateTime>(start, end));
                    }
                }                
            }
            else if (index == 4)
            {
                Status = (AutoAnswerStatus) Convert.ToInt32(value);
            }
            else if (index == 5)
            {
                NumberOfCalls = Convert.ToInt32(value);
            }
            else if (index == 6)
            {
                NumberOfRingsInListeningWindow = NumberOfRingsOutListeningWindow = 0;
                if (value != null)
                {
                    NumberOfRingsInListeningWindow = Convert.ToInt32(((Object[])value)[0]);
                    NumberOfRingsOutListeningWindow = Convert.ToInt32(((Object[])value)[1]);
                }
            }
            else
            {
                throw new ArgumentException("SetValue failed. Invalid attribute index.");
            }
        }

        byte[][] IGXDLMSBase.Invoke(object sender, int index, Object parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
