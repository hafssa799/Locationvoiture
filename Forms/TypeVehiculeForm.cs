using System;
using System.IO;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class TypeVehiculeForm : Form
    {
        private Repository repo;
        private TypeVehicule typeVehicule;
        private bool isEdit;
        private byte[] imageBytes;

        public TypeVehiculeForm(TypeVehicule typeVehicule = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.typeVehicule = typeVehicule;
            isEdit = typeVehicule != null;

            if (isEdit)
            {
                this.Text = "Modifier Type de Véhicule";
                LoadType();
            }
            else
            {
                this.Text = "Nouveau Type de Véhicule";
                this.typeVehicule = new TypeVehicule();
            }
        }

        private TextBox txtNom, txtDescription;
        private PictureBox picImage;
        private Button btnSelectImage, btnSave, btnCancel;

        private void InitializeComponent()
        {
            var lblNom = new Label { Text = "Nom:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblDescription = new Label { Text = "Description:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblImage = new Label { Text = "Image:", Location = new System.Drawing.Point(20, 150), AutoSize = true };

            txtNom = new TextBox { Location = new System.Drawing.Point(120, 27), Size = new System.Drawing.Size(300, 20) };
            txtDescription = new TextBox { Location = new System.Drawing.Point(120, 67), Size = new System.Drawing.Size(300, 100), Multiline = true };
            picImage = new PictureBox { Location = new System.Drawing.Point(120, 147), Size = new System.Drawing.Size(200, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            btnSelectImage = new Button { Text = "Sélectionner image", Location = new System.Drawing.Point(330, 147), Size = new System.Drawing.Size(120, 30) };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 320), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 320), Size = new System.Drawing.Size(100, 35) };

            btnSelectImage.Click += BtnSelectImage_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblNom, lblDescription, lblImage,
                txtNom, txtDescription, picImage, btnSelectImage, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(480, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageBytes = File.ReadAllBytes(dialog.FileName);
                    picImage.Image = System.Drawing.Image.FromFile(dialog.FileName);
                    typeVehicule.NomImage = Path.GetFileName(dialog.FileName);
                }
            }
        }

        private void LoadType()
        {
            txtNom.Text = typeVehicule.Nom;
            txtDescription.Text = typeVehicule.Description;
            if (typeVehicule.Image != null && typeVehicule.Image.Length > 0)
            {
                using (var ms = new MemoryStream(typeVehicule.Image))
                {
                    picImage.Image = System.Drawing.Image.FromStream(ms);
                }
                imageBytes = typeVehicule.Image;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom est obligatoire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            typeVehicule.Nom = txtNom.Text;
            typeVehicule.Description = txtDescription.Text;

            bool success = isEdit ? repo.UpdateTypeVehicule(typeVehicule, imageBytes) : repo.AddTypeVehicule(typeVehicule, imageBytes);
            if (success)
            {
                MessageBox.Show(isEdit ? "Type modifié avec succès." : "Type ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Une erreur est survenue.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

