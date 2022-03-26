using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Samostalan_rad_2
{
    public partial class Glavna_forma : Form
    {
        public Glavna_forma()
        {
            InitializeComponent();
        }


        //imena elemenata menija nisu mijenjana jer su mi i ovako smislena
        //to isto važi i za kontekstni meni
        //richtextbox je nazvan radna površina
        //mada je mogao ostati i default naziv jer se zna tačno na šta se misli
        //pošto postoji samo jedan

        #region MENU

        //poziva funkciju zatvaranje()
        //moglo se realizovati i samo kao Application.Close();
        //ili this.Close();
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zatvaranje();
        }

        //kreira novi fajl
        //ako nije ništa upisano samo nastavlja da radi na postojećoj radnoj površini
        //ako je otvoren neki fajl koji je prazan onda samo čisti ime forme/direktorijuma
        //ako je nešto upisano nudi mogućnost da se to sačuva a onda čisti radnu površinu i ime forme/direktorijuma
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (radnaPovrsina.Text != "")
            {
                DialogResult potvrdi = MessageBox.Show("Želite li da sačuvate izmjene?", "Sačuvaj?", MessageBoxButtons.YesNoCancel);
                if (potvrdi == DialogResult.Yes)
                {
                    SaveFileDialog sacuvaj = new SaveFileDialog();
                    sacuvaj.Filter = "Rich Text Files (*.rtf)|*.rtf";
                    sacuvaj.ShowDialog();
                    if (sacuvaj.FileName != "")
                    {
                        radnaPovrsina.SaveFile(sacuvaj.FileName, RichTextBoxStreamType.RichText);
                        this.Text = sacuvaj.FileName;
                    }
                    radnaPovrsina.Text = "";
                    this.Text = "New Document";
                }
                if(potvrdi==DialogResult.No)
                {
                    radnaPovrsina.Text = "";
                }
            }
            if (radnaPovrsina.Text == "" && this.Text != "New Document")
            {
                this.Text = "New Document";
            }
        }

        //otvara  postojeći fajl
        //moglo se uraditi i preko StreamReader ali meni se ovako više svidja
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog otvori = new OpenFileDialog();
            otvori.Filter = "Rich Text Files (*.rtf)|*.rtf";
            otvori.ShowDialog();
            if (otvori.FileName != "")
            {
                radnaPovrsina.LoadFile(otvori.FileName, RichTextBoxStreamType.RichText);
                this.Text = otvori.FileName;
            }
           /* StreamReader open = new StreamReader (otvori.FileName);
            radnaPovrsina.Text= open.ReadToEnd();*/

        }

        //čuva fajl
        //moglo je i preko FileStream ali mi se ovako više svidja
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sacuvaj = new SaveFileDialog();
            sacuvaj.Filter = "Rich Text Files (*.rtf)|*.rtf";
            sacuvaj.ShowDialog();
            if (sacuvaj.FileName != "")
            {
                radnaPovrsina.SaveFile(sacuvaj.FileName, RichTextBoxStreamType.RichText);
                this.Text = sacuvaj.FileName;

               /* FileStream fParameter = new FileStream(sacuvaj.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter m_WriterParameter = new StreamWriter(fParameter);
                m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
                m_WriterParameter.Flush();
                m_WriterParameter.Close();*/
            }

        }

        //kopira pozivom funkcije copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy();
        }

        //lijepi pozivom funkcije paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paste();
        }

        //isjeca pozivom funkcije cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cut();
        }

        //selektuje sve karaktere na radnoj površini
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radnaPovrsina.SelectAll();
        }

        //otvara dialog za izbor boja
        private void allColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog paleta=new ColorDialog();
            paleta.ShowDialog();
            radnaPovrsina.SelectionColor = paleta.Color;

         
        }

        //podešava boju
        //poziva se klikom na neku od ponuđenih boja u meniju
        private void boja(object sender, EventArgs e)
        {
            ToolStripMenuItem miColor = (ToolStripMenuItem)sender;
            radnaPovrsina.SelectionColor = Color.FromName(miColor.Text); 
        }

        //otvara dialog za izbor fonta
        private void allFontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontovi = new FontDialog();
            fontovi.ShowDialog();
            radnaPovrsina.SelectionFont = fontovi.Font;
        }

        //podešava stil u zavisnosti od toga šta je izabrano
        //poziva se klikom na neki od stilova u meniju: bold,italic,underline
        private void stil(object sender, EventArgs e)
        {
            ToolStripMenuItem miFont = (ToolStripMenuItem)sender;
            radnaPovrsina.SelectionFont = miFont.Font;
        }
        #endregion

        #region contextMenu
        //funkcije kontekstnog menija

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cut();
        }    

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            paste();
        }
        #endregion


        #region COPY,CUT,PASTE
        //uradjeni kao funkcije koje se pozivaju iz "menu" ili "contextMenuStrip1"
        //TextDataFormat.UnicodeText da bi radilo sa ćirilicom

        private void cut()
        {
            if (radnaPovrsina.SelectedText != "")
                Clipboard.SetText(radnaPovrsina.SelectedText, TextDataFormat.UnicodeText);
            radnaPovrsina.SelectedText = "";

        }

        private void copy()
        {
            if (radnaPovrsina.SelectedText != "")
                Clipboard.SetText(radnaPovrsina.SelectedText, TextDataFormat.UnicodeText);
            
            

        }

        private void paste()
        {
            if (Clipboard.GetText() != null)
                radnaPovrsina.AppendText(Clipboard.GetText(TextDataFormat.UnicodeText));

        }
        #endregion


        //Otvara kontekstni meni ako je bio desni klik
        private void radnaPovrsina_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show();
        }


        //funkcija koja otvara dialog za cuvanje prije izlaska
        //nije se trazila ali moze i ovako nesto da se uradi
        //radi samo ako se izlazi iz menija
        //ako se zatvara na "X" onda ne radi
        //ali moglo bi i to da se napravi malo mi komplikovano a svakako se nije trazilo ovo je čisto malo za vježbu
        private void zatvaranje()
        {
            string poredi_rp = radnaPovrsina.Text;
            string poredi_fajl = "";
            string rich_text="";
            if (File.Exists(this.Text))
            {
               
                RichTextBox rtBox = new RichTextBox();
                rich_text = File.ReadAllText(this.Text);
                rtBox.Rtf = rich_text;
                poredi_fajl = rtBox.Text;
            }
            if (poredi_fajl == poredi_rp)
                Application.Exit();
            if (!(string.Equals(poredi_fajl,poredi_rp)))
            {
               
                DialogResult potvrdi = MessageBox.Show("Želite li da sačuvate izmjene?", "Zatvori aplikaciju", MessageBoxButtons.YesNoCancel);

                if (potvrdi == DialogResult.Yes)
                {
                    if (this.Text != "New Document")
                    {
                        radnaPovrsina.SaveFile(this.Text, RichTextBoxStreamType.RichText);
                        Application.Exit();
                    }
                    if (this.Text == "New Document")
                    {
                        SaveFileDialog sacuvaj = new SaveFileDialog();
                        sacuvaj.Filter = "Rich Text Files (*.rtf)|*.rtf";
                        sacuvaj.ShowDialog();
                        if (sacuvaj.FileName != "")
                        {
                            this.Text = sacuvaj.FileName;
                            radnaPovrsina.SaveFile(sacuvaj.FileName, RichTextBoxStreamType.RichText);
                            Application.Exit();
                        }
                    }

                }

                if (potvrdi == DialogResult.No)
                    Application.Exit();
            }
            
          
        }

        private void menu_File_Click(object sender, EventArgs e)
        {

        }

        
          

    }
}
