using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Project19_TableStatusMomentary
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      timer1.Start();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      Db19Project20Entities1 context = new Db19Project20Entities1();

      var buttons = this.Controls.OfType<Button>().ToList();

      foreach (var btn in buttons)
      {
        this.Controls.Remove(btn);
      }

      var values = context.TblTables.ToList();

      int buttonWidth = 100;
      int buttonHeight = 50;
      int padding = 10;
      int xOffset = 10;
      int yOffset = 250;

      for (int i = 0; i < values.Count; i++)
      {
        var item = values[i];
        Button button = new Button();
        button.Text = item.TableNumber.ToString();
        button.Size = new Size(buttonWidth, buttonHeight);
        button.Location = new Point(xOffset + (i % 4) * (buttonWidth + padding),
                                    yOffset + (i / 4) * (buttonHeight + padding));
        
        bool isFull = Convert.ToBoolean(item.Status);
        button.BackColor = isFull ? Color.Red : Color.Green;
        button.Tag = item.TableId; 

        if (isFull)
        {
          button.Click += TableButton_Click;
        }

        this.Controls.Add(button);
      }
    }

      private void TableButton_Click(object sender, EventArgs e)
    {
      Button clickedButton = sender as Button;
      if (clickedButton == null) return;

      int tableId = (int)clickedButton.Tag;

      using (Db19Project20Entities1 context = new Db19Project20Entities1())
      {
        var detail = context.TblTableDetails
                            .FirstOrDefault(d => d.TableId == tableId);

        if (detail != null)
        {
          string message = $"Masa No: {detail.TblTables.TableNumber}\n"+ $"Müşteri: {detail.CustomerName}\n" + $"Tutar: {detail.TotalPrice} ₺\n" + $"Müşteri Notu: {detail.Note}\n";

          MessageBox.Show(message, "Masa Detayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          MessageBox.Show("Bu masaya ait detay bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
    }
  }  
}

