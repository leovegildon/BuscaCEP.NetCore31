using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BuscaCEP
{
    public partial class Form1 : Form
    {

        public class DadosRetornadosAPI
        {
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }
            public string ibge { get; set; }
            public string gia { get; set; }
            public string ddd { get; set; }
            public string siafi { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (txtCEPEntrada.TextLength < 8)
            { MessageBox.Show("Entre com um CEP válido.", "Formato incorreto", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            else
            {
                txtLogradouro.Text = "";
                txtComplemento.Text = "";
                txtBairro.Text = "";
                txtMunicipio.Text = "";
                txtUF.Text = "";
                txtIBGE.Text = "";
                txtDDD.Text = "";
                try
                {

                    Uri uri = new Uri(@"https://viacep.com.br/ws/" + txtCEPEntrada.Text + "/json/");
                    WebRequest webRequest = WebRequest.Create(uri);
                    WebResponse response = webRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    String responseData = streamReader.ReadToEnd();
                    DadosRetornadosAPI dados = JsonConvert.DeserializeObject<DadosRetornadosAPI>(responseData);

                    txtLogradouro.Text = dados.logradouro;
                    txtComplemento.Text = dados.complemento;
                    txtBairro.Text = dados.bairro;
                    txtMunicipio.Text = dados.localidade;
                    txtUF.Text = dados.uf;
                    txtIBGE.Text = dados.ibge;
                    txtDDD.Text = dados.ddd;

                    if (txtLogradouro.Text == "")
                    { txtLogradouro.Text = "CEP não encontrado!"; }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro na consulta. Provavelmente o provedor de dados ViaCEP está indisponível \n\n" + ex.Message, 
                        "Erro na consulta", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }
        }

        private void txtCEPEntrada_KeyPress(object sender, KeyPressEventArgs e)
        {
        if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
               {
                   e.Handled = true;
               }

            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                btnConsultar_Click(sender, e);
            }
        }


    }
}
