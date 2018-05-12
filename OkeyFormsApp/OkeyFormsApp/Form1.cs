using OkeyFormApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OkeyFormsApp
{
    public partial class Form1 : Form
    {
        private static List<Tile> usedTiles { get; } = new List<Tile>();
        private static List<Tile> allTiles { get; } = new List<Tile>();
        private static List<List<Tile>> allPlayerTiles { get; } = new List<List<Tile>>();
        private static Random random { get; } = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartTheGame();
            lblStartInfo.Text = "Click the start button for the new game...";
        }

        private void StartTheGame()
        {
            flp1.Controls.Clear();
            flp2.Controls.Clear();
            flp3.Controls.Clear();
            flp4.Controls.Clear();

            flpEdit1.Controls.Clear();
            flpEdit2.Controls.Clear();
            flpEdit3.Controls.Clear();
            flpEdit4.Controls.Clear();

            if (usedTiles.Any()) usedTiles.Clear();
            if (allTiles.Any()) allTiles.Clear();
            if (allPlayerTiles.Any()) allPlayerTiles.Clear();

            int color = random.Next(1, EnumConstant.ColorCount + 1);
            int value = random.Next(1, EnumConstant.PerColorTileCount + 1);

            Tile faceUpTile = new Tile(value, color);
            usedTiles.Add(faceUpTile);
            lblFaceUpTile.Text = faceUpTile.GetValue().ToString();
            lblFaceUpTile.ForeColor = faceUpTile.GetColor(faceUpTile.GetColor());

            CreateAllTiles(faceUpTile);
            ShuffleAndDistrubuteTiles(faceUpTile);

            int playerNo = 1;

            foreach (var playerTiles in allPlayerTiles)
            {
                foreach (var tile in playerTiles)
                {
                    Label lblTile = new Label();
                    lblTile.Width = 40;
                    lblTile.ForeColor = tile.GetColor(tile.GetColor());

                    if (tile.IsOkey())
                        lblTile.Text = "OK";
                    else if (tile.IsFakeOkey())
                        lblTile.Text = string.Concat(tile.GetValue().ToString(), "(*)");
                    else
                        lblTile.Text = tile.GetValue().ToString();

                    if (playerNo == 1)
                        flp1.Controls.Add(lblTile);
                    else if (playerNo == 2)
                        flp2.Controls.Add(lblTile);
                    else if (playerNo == 3)
                        flp3.Controls.Add(lblTile);
                    else
                        flp4.Controls.Add(lblTile);
                }
                playerNo++;
            }
        }

        private static void CreateAllTiles(Tile faceUpTile)
        {
            foreach (EnumConstant.Color color in Enum.GetValues(typeof(EnumConstant.Color)))
            {
                for (int value = 1; value <= EnumConstant.PerColorTileCount; value++)
                {
                    bool isOkey = Tile.IsOkeyFromFaceUpTile(new Tile(value, (int)color), faceUpTile);

                    allTiles.Add(new Tile(value, (int)color, isOkey));
                    allTiles.Add(new Tile(value, (int)color, isOkey));
                }
            }

            allTiles.Add(Tile.SetFakeOkeyFromFaceUpTile(faceUpTile));
            allTiles.Add(Tile.SetFakeOkeyFromFaceUpTile(faceUpTile));
        }

        private static void ShuffleAndDistrubuteTiles(Tile faceUpTile)
        {
            int firstPlayer = random.Next(1, EnumConstant.PlayerCount + 1);

            for (int pc = 1; pc <= EnumConstant.PlayerCount; pc++)
            {
                List<Tile> playerTiles = new List<Tile>();

                for (int tc = 1; tc <= EnumConstant.PerPlayerTileCount; tc++)
                {
                    Tile newTile = null;
                    newTile = GetTile(newTile, faceUpTile);
                    playerTiles.Add(newTile);
                }

                if (pc == firstPlayer)
                {
                    Tile newTile = null;
                    newTile = GetTile(newTile, faceUpTile);
                    playerTiles.Add(newTile);
                }

                allPlayerTiles.Add(playerTiles);
            }
        }

        private static Tile GetTile(Tile newTile, Tile faceUpTile)
        {
            while (newTile == null)
            {
                int color = random.Next(1, EnumConstant.ColorCount + 1);
                int value = random.Next(1, EnumConstant.PerColorTileCount + 1);

                bool isOkOrFakeOkey = Tile.IsOkeyFromFaceUpTile(new Tile(value, color), faceUpTile);
                int tileCount = isOkOrFakeOkey ? EnumConstant.PerTileCount * 2 : EnumConstant.PerTileCount;

                bool isFinished = (usedTiles.AsEnumerable().Where(p => p.GetColor() == color && p.GetValue() == value).Select(p => p).Count() == tileCount);

                if (!isFinished)
                {
                    newTile = allTiles.AsEnumerable().Where(p => p.GetColor() == color && p.GetValue() == value).Select(p => p).FirstOrDefault();
                    if (newTile != null)
                    {
                        usedTiles.Add(newTile);
                        allTiles.Remove(newTile);
                    }
                }
            }

            return newTile;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            flpEdit1.Controls.Clear();
            flpEdit2.Controls.Clear();
            flpEdit3.Controls.Clear();
            flpEdit4.Controls.Clear();

            int playerNo = 1;

            foreach (var playerTiles in allPlayerTiles)
            {
                List<Tile> editedTiles = EditTiles(playerTiles);

                foreach (var tile in editedTiles)
                {
                    Label lblTile = new Label();
                    lblTile.Width = 40;
                    lblTile.ForeColor = tile.GetColor(tile.GetColor());

                    if (tile.IsOkey())
                        lblTile.Text = "OK";
                    else if (tile.IsFakeOkey())
                        lblTile.Text = string.Concat(tile.GetValue().ToString(), "(*)");
                    else
                        lblTile.Text = tile.GetValue().ToString();

                    if (playerNo == 1)
                        flpEdit1.Controls.Add(lblTile);
                    else if (playerNo == 2)
                        flpEdit2.Controls.Add(lblTile);
                    else if (playerNo == 3)
                        flpEdit3.Controls.Add(lblTile);
                    else
                        flpEdit4.Controls.Add(lblTile);

                }
                playerNo++;
            }
        }

        private static List<Tile> EditTiles(List<Tile> tiles)
        {
            List<Tile> editedTiles = new List<Tile>();

            List<Tile> sameValTiles = GetSameValueTiles(tiles, EnumConstant.MinSerieTileCount);
            if (sameValTiles.Any())
                editedTiles.AddRange(sameValTiles);

            List<Tile> sequencedTiles = GetSameColorSequencedTiles(tiles, sameValTiles);
            if (sequencedTiles.Any())
                editedTiles.AddRange(sequencedTiles);

            return editedTiles;
        }

        private static List<Tile> GetSameValueTiles(List<Tile> tiles, int minTileCount)
        {
            List<Tile> newTiles = new List<Tile>();

            for (int value = 1; value <= EnumConstant.PerColorTileCount; value++)
            {
                if (tiles.AsEnumerable().Where(p => p.GetValue() == value).Select(p => p).Count() >= minTileCount)
                    newTiles.AddRange(tiles.AsEnumerable().Where(p => p.GetValue() == value).Select(p => p).ToList());
            }
            return newTiles;
        }

        private static List<Tile> GetSameColorSequencedTiles(List<Tile> tiles, List<Tile> usedTiles)
        {
            List<Tile> newTiles = new List<Tile>();

            foreach (EnumConstant.Color color in Enum.GetValues(typeof(EnumConstant.Color)))
            {
                var sameColorTiles = tiles.AsEnumerable().Where(p => p.GetColor() == (int)color).Select(p => p).OrderBy(p => p.GetValue()).ToList();
                foreach (var tile in sameColorTiles)
                {
                    if (!usedTiles.AsEnumerable().Where(p => p.GetValue() == tile.GetValue() && p.GetColor() == tile.GetColor()).Select(p => p).Any())
                        newTiles.Add(tile);
                }
            }
            return newTiles;
        }
    }
}
