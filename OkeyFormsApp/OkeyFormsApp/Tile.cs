using System.Drawing;

namespace OkeyFormApp
{
    public class Tile
    {
        private int value { get; set; }
        private int color { get; set; }
        private bool fakeOkey { get; set; }
        private bool okey { get; set; }

        public Tile(int value, int color)
        {
            this.value = value;
            this.color = color;
            this.fakeOkey = false;
            this.okey = false;
        }

        public Tile(int value, int color, bool isOkey)
        {
            this.value = value;
            this.color = color;
            this.fakeOkey = false;
            this.okey = isOkey;
        }

        public Tile(Tile tile)
        {
            this.color = tile.color;
            this.value = tile.value;
            this.fakeOkey = tile.fakeOkey;
            this.okey = tile.okey;
        }

        public static Tile SetFakeOkeyFromFaceUpTile(Tile tile)
        {
            int fakeOkeyColor = tile.GetColor();
            int fakeOkeyValue = (tile.GetValue() + 1 > EnumConstant.PerColorTileCount) ? 1 : tile.GetValue() + 1;
            return SetFakeOkey(fakeOkeyValue, fakeOkeyColor);
        }

        private static Tile SetFakeOkey(int value, int color)
        {
            Tile tile = new Tile(value, color);
            tile.fakeOkey = true;
            tile.okey = false;
            return tile;
        }

        public bool IsFakeOkey()
        {
            return this.fakeOkey;
        }

        public bool IsOkey()
        {
            return this.okey;
        }

        public static bool IsOkeyFromFaceUpTile(Tile tile, Tile faceUpTile)
        {
            int okeyValue = faceUpTile.GetValue() + 1 > EnumConstant.PerColorTileCount ? 1 : faceUpTile.GetValue() + 1;
            return (tile.GetColor() == faceUpTile.GetColor() && tile.GetValue() == okeyValue);
        }

        public int GetValue()
        {
            return value;
        }

        public int GetColor()
        {
            return color;
        }

        public Color GetColor(int color)
        {
            switch (color)
            {
                case (int)EnumConstant.Color.Red:
                    return Color.Red;

                case (int)EnumConstant.Color.Black:
                    return Color.Black;

                case (int)EnumConstant.Color.Yellow:
                    return Color.Gold;

                case (int)EnumConstant.Color.Blue:
                    return Color.Blue;

                default:
                    return Color.White;
            }
        }
    }
}
