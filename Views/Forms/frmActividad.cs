using System;
using System.Windows.Forms;
using ClubDeportivo.Models;
using ClubDeportivo.Repositories;

namespace ClubDeportivo.Views.Forms
{
    public partial class frmActividad : Form
    {
        // Repositorio para manejar la persistencia de Actividad
        private readonly ActividadRepository _actividadRepository;

        // Constructor que recibe el repositorio por inyección de dependencias
        public frmActividad(ActividadRepository actividadRepository)
        {
            InitializeComponent();
            _actividadRepository = actividadRepository;
        }

        // Evento click del botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validación: el campo Nombre no puede estar vacío o solo espacios
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return; // Detiene la ejecución para que el usuario corrija
            }

            try
            {
                // Crear un objeto Actividad con los datos del formulario
                var actividad = new Actividad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Horario = txtHorario.Text.Trim(),
                    Precio = Convert.ToDecimal(txtPrecio.Value),
                    ExclusivaSocios = chkExclusiva.Checked
                };

                // Guardar la actividad mediante el repositorio
                _actividadRepository.Agregar(actividad);

                // Informar éxito al usuario
                MessageBox.Show("Actividad guardada correctamente", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK; // Indicar que el diálogo terminó OK
                Close(); // Cerrar el formulario
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la operación
                MessageBox.Show(string.Format("Error al guardar la actividad:{0}", ex.Message), "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
