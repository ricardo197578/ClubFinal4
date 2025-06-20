using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Views.Forms
{
    public partial class frmActividad : Form
    {
        private readonly ActividadRepository _actividadRepository;

        public frmActividad(ActividadRepository actividadRepository)
        {
            InitializeComponent();
            _actividadRepository = actividadRepository;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return;
            }

            try
            {
                var actividad = new Actividad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Horario = txtHorario.Text.Trim(),
                    Precio = Convert.ToDecimal(txtPrecio.Value),
                    ExclusivaSocios = chkExclusiva.Checked
                };

                _actividadRepository.Agregar(actividad);
                MessageBox.Show("Actividad guardada correctamente", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al guardar la actividad:{0}",ex.Message), "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}