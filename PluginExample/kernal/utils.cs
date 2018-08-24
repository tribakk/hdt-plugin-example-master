using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT = Hearthstone_Deck_Tracker;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PluginExample.kernal
{
    class utils
    {
        internal static HearthDb.Enums.CardClass KlassConverter(string klass)
        {
            switch (klass.ToLowerInvariant())
            {
                case "druid":
                    return HearthDb.Enums.CardClass.DRUID;

                case "hunter":
                    return HearthDb.Enums.CardClass.HUNTER;

                case "mage":
                    return HearthDb.Enums.CardClass.MAGE;

                case "paladin":
                    return HearthDb.Enums.CardClass.PALADIN;

                case "priest":
                    return HearthDb.Enums.CardClass.PRIEST;

                case "rogue":
                    return HearthDb.Enums.CardClass.ROGUE;

                case "shaman":
                    return HearthDb.Enums.CardClass.SHAMAN;

                case "warlock":
                    return HearthDb.Enums.CardClass.WARLOCK;

                case "warrior":
                    return HearthDb.Enums.CardClass.WARRIOR;

                default:
                    return HearthDb.Enums.CardClass.NEUTRAL;
            }
        }

        internal static void SortDeck(List<Hearthstone_Deck_Tracker.Hearthstone.Card> cardList)
        {
            for (int i = 0; i < cardList.Count - 1; i++)
            {
                for (int j = 0; j < cardList.Count - 1; j++)
                {
                    if (cardList[j].Cost > cardList[j + 1].Cost)
                    {
                        var temp = cardList[j];
                        cardList[j] = cardList[j + 1];
                        cardList[j + 1] = temp;
                    }
                }
            }
        }

        internal static List<Hearthstone_Deck_Tracker.Hearthstone.Card> CardDictinaryToList(Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> cardDict)
        {
            List<Hearthstone_Deck_Tracker.Hearthstone.Card> cardList = new List<Hearthstone_Deck_Tracker.Hearthstone.Card>();
            var pEnum = cardDict.GetEnumerator();
            for (; pEnum.MoveNext();)
            {
                cardList.Add(pEnum.Current.Key);
            }
            SortDeck(cardList);
            return cardList;
        }

        internal static Size DrawCard(HDT.Hearthstone.Card card, int cardCount, Graphics pGraphics)
        {
            Bitmap ImageBmp = HDT.Utility.ImageCache.GetCardBitmap(card);
            SetAlphaChannel(ImageBmp);
            int HWSize = ImageBmp.Height;
            int FullWidth = 200 + HWSize;
            int TopPosition = 6;
            Pen RoundPer = new Pen(new SolidBrush(Color.Black));
            
            Font pTextFont = new Font("Times New Roman", 16, FontStyle.Bold);
            {
                Pen pen = new Pen(Color.Black);
                SolidBrush brush = new SolidBrush(Color.FromArgb(255,53,84,118));
                
                pGraphics.FillRectangle(brush, 0, 0, HWSize, HWSize);
                brush.Color = Color.White;
                pGraphics.DrawRectangle(RoundPer, 0,0, HWSize, HWSize);
                pGraphics.DrawString(card.Cost.ToString(), pTextFont, brush, new Point(8, TopPosition));
            }
            SolidBrush pBackgroundBrush = new SolidBrush(Color.FromArgb(255, 53, 53, 24));
            pGraphics.FillRectangle(pBackgroundBrush, HWSize, 0, FullWidth, HWSize);
            pGraphics.DrawImage(ImageBmp, new Point(FullWidth - ImageBmp.Width, 0));
            pGraphics.DrawString(card.Name, new Font("Times New Roman", 12, FontStyle.Bold), new SolidBrush(Color.White), new Point(HWSize, TopPosition + 2));
            if (cardCount > 1)
            {
                SolidBrush brushBackground = new SolidBrush(Color.FromArgb(255, 53, 53, 53));
                SolidBrush brushCardCount = new SolidBrush(Color.FromArgb(255, 255, 214, 0));
                int StartX = FullWidth - HWSize;
                pGraphics.DrawRectangle(RoundPer, StartX, 0, FullWidth, HWSize);
                pGraphics.FillRectangle(brushBackground, StartX, 0, FullWidth, HWSize);
                pGraphics.DrawString(cardCount.ToString(), pTextFont, brushCardCount, new Point(StartX + 8, TopPosition));
            }
            pGraphics.DrawRectangle(RoundPer, 0, 0, FullWidth, HWSize);
            return new Size(FullWidth, HWSize);
        }

        internal static void SetAlphaChannel(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                for (int counter = 3; counter < rgbValues.Length; counter += 4)
                {
                    double power = 2;
                    double place1value = 0.4;
                    double k = 1/Math.Pow(place1value, power);
                    double alfaPercent = counter % bmpData.Stride + 1;
                    alfaPercent = alfaPercent / bmpData.Stride;
                    alfaPercent = Math.Pow(alfaPercent, power) * k;
                    alfaPercent = Math.Min(1.0, alfaPercent);
                    byte value = (byte)((double)alfaPercent  * 255);
                    rgbValues[counter] = value;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        internal static Bitmap GetBitmap(System.Windows.Media.Imaging.BitmapSource source)
        {
            Bitmap bmp = new Bitmap (source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            source.CopyPixels (System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);

            bmp.UnlockBits(data);

            return bmp;
        }

        public static System.Windows.Media.Imaging.BitmapSource Generate(HDT.Hearthstone.Deck deck, bool cardsOnly)
        {
            const int CardHeight = 35;
            const int InfoHeight = 124;
            const int ScreenshotWidth = 219;
            const int Dpi = 96;

            var height = CardHeight * deck.GetSelectedDeckVersion().Cards.Count;
            if (!cardsOnly)
                height += InfoHeight;
            var control = new HDT.Controls.DeckView(deck, cardsOnly);
            control.Measure(new System.Windows.Size(ScreenshotWidth, height));
            control.Arrange(new System.Windows.Rect(new System.Windows.Size(ScreenshotWidth, height)));
            control.UpdateLayout();
            //Log.Debug($"Screenshot: {control.ActualWidth} x {control.ActualHeight}");
            var bmp = new System.Windows.Media.Imaging.RenderTargetBitmap(ScreenshotWidth, height, Dpi, Dpi, System.Windows.Media.PixelFormats.Pbgra32);
            bmp.Render(control);
            return bmp;
        }

    }
}
