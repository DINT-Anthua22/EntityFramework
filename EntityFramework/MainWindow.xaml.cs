using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EntityFramework
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CLIENTE cliente_plantilla;
        CollectionViewSource vista;
        InformesEntities contexto;
        public MainWindow()
        {
            InitializeComponent();
            contexto = new InformesEntities();
            contexto.CLIENTES.Include(x => x.PEDIDOS).Load();
            cliente_plantilla = new CLIENTE();
            ContenedorStackPanel.DataContext = cliente_plantilla;
            ListaListBox.DataContext = contexto.CLIENTES.Local;
            UsuariosCombox.DataContext = contexto.CLIENTES.Local;
            ClientesDataGrid.DataContext = contexto.CLIENTES.Local;
            vista = new CollectionViewSource();
            
            vista.Source = contexto.CLIENTES.Local;
            ClientesFiltrarDataGrid.DataContext = vista;
            vista.Filter += Vista_Filter;
            UsuariosModificarCombox.DataContext = contexto.CLIENTES.Local;
        }

        private void InsertarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contexto.CLIENTES.Add(cliente_plantilla);
               
                contexto.SaveChanges();
                cliente_plantilla = new CLIENTE();
                ContenedorStackPanel.DataContext = cliente_plantilla;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            if(UsuariosCombox.SelectedItem!= null)
            {
                if(((CLIENTE)UsuariosCombox.SelectedItem).PEDIDOS.Count == 0)
                {
                    contexto.CLIENTES.Remove((CLIENTE)UsuariosCombox.SelectedItem);
                    contexto.SaveChanges();
                }
               
            }
            
        }

        private void ModificarButton_Click(object sender, RoutedEventArgs e)
        {
            contexto.SaveChanges();
        }

        private void ActualizarButton_Click(object sender, RoutedEventArgs e)
        {
            contexto.SaveChanges();
        }

        private void FiltarButton_Click(object sender, RoutedEventArgs e)
        {
            vista.View.Refresh();

        }

        private void Vista_Filter(object sender, FilterEventArgs e)
        {
            CLIENTE item =(CLIENTE)e.Item;
            if (FiltroNombreTextBox.Text == "")
            {
                e.Accepted=true;
            }
            else
            {
                if(item.nombre.Contains(FiltroNombreTextBox.Text)|| item.apellido.Contains(FiltroNombreTextBox.Text))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }
    }
}
