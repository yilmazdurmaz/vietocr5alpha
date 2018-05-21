/**
 * Copyright @ 2008 Quan Nguyen
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
//using Vietpad.NET.Controls;
using Microsoft.Win32;
using Tesseract;
using System.Windows.Controls;
using System.Windows;

namespace VietOCR
{
    public class GuiWithPSM : GuiWithBatch
    {
        const string strPSM = "PageSegMode";
        MenuItem psmItemChecked;

        public GuiWithPSM()
        {
            Dictionary<string, string> psmDict = new Dictionary<string, string>();
            psmDict.Add("OsdOnly", "0 - Orientation and script detection (OSD) only");
            psmDict.Add("AutoOsd", "1 - Automatic page segmentation with OSD");
            psmDict.Add("AutoOnly", "2 - Automatic page segmentation, but no OSD, or OCR");
            psmDict.Add("Auto", "3 - Fully automatic page segmentation, but no OSD (default)");
            psmDict.Add("SingleColumn", "4 - Assume a single column of text of variable sizes");
            psmDict.Add("SingleBlockVertText", "5 - Assume a single uniform block of vertically aligned text");
            psmDict.Add("SingleBlock", "6 - Assume a single uniform block of text");
            psmDict.Add("SingleLine", "7 - Treat the image as a single text line");
            psmDict.Add("SingleWord", "8 - Treat the image as a single word");
            psmDict.Add("CircleWord", "9 - Treat the image as a single word in a circle");
            psmDict.Add("SingleChar", "10 - Treat the image as a single character");
            psmDict.Add("SparseText", "11 - Find as much text as possible in no particular order");
            psmDict.Add("SparseTextOsd", "12 - Sparse text with orientation and script detection");
            psmDict.Add("RawLine", "13 - Treat the image as a single text line, bypassing hacks that are Tesseract-specific");
            psmDict.Add("Count", "14 - Number of enum entries");

            //
            // Settings PageSegMode submenu
            //
            RoutedEventHandler eh = new RoutedEventHandler(MenuPSMOnClick);

            foreach (string mode in Enum.GetNames(typeof(PageSegMode)))
            {
                if ((PageSegMode)Enum.Parse(typeof(PageSegMode), mode) == PageSegMode.Count)
                {
                    continue;
                }
                MenuItem psmItem = new MenuItem();
                psmItem.Header = psmDict[mode];
                psmItem.Tag = mode;
                //psmItem.CheckOnClick = true;
                psmItem.Click += eh;
                this.psmToolStripMenuItem.Items.Add(psmItem);
            }
        }

        protected override void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);

            for (int i = 0; i < this.psmToolStripMenuItem.Items.Count; i++)
            {
                if (((MenuItem)this.psmToolStripMenuItem.Items[i]).Tag.ToString() == selectedPSM)
                {
                    // Select PSM last saved
                    psmItemChecked = (MenuItem)psmToolStripMenuItem.Items[i];
                    psmItemChecked.IsChecked = true;
                    break;
                }
            }

            this.statusLabelPSMvalue.Content = selectedPSM;
        }

        void MenuPSMOnClick(object obj, EventArgs ea)
        {
            if (psmItemChecked != null)
            {
                psmItemChecked.IsChecked = false;
            }
            psmItemChecked = (MenuItem)obj;
            psmItemChecked.IsChecked = true;
            selectedPSM = psmItemChecked.Tag.ToString();
            this.statusLabelPSMvalue.Content = selectedPSM;
        }

        protected override void LoadRegistryInfo(RegistryKey regkey)
        {
            base.LoadRegistryInfo(regkey);
            selectedPSM = (string)regkey.GetValue(strPSM, Enum.GetName(typeof(PageSegMode), Tesseract.PageSegMode.Auto));
            try
            {
                // validate PSM value
                Tesseract.PageSegMode psm = (PageSegMode)Enum.Parse(typeof(PageSegMode), selectedPSM);
            }
            catch
            {
                selectedPSM = Enum.GetName(typeof(PageSegMode), Tesseract.PageSegMode.Auto);
            }
        }

        protected override void SaveRegistryInfo(RegistryKey regkey)
        {
            base.SaveRegistryInfo(regkey);
            regkey.SetValue(strPSM, selectedPSM);
        }
    }
}
