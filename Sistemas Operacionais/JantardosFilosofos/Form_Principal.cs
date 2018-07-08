using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JantardosFilosofos
{
    public partial class Form_Principal : Form
    {
        public Form_Principal()
        {
            InitializeComponent();
        }
        private static Semaphore Semaforo;
        Thread F1, F2, F3, F4, F5;
        Garfo[] Garfo = new JantardosFilosofos.Garfo[5];
        Filosofo[] Filosofos = new JantardosFilosofos.Filosofo[5];
        bool controlaThreads = true;
        int controlaTempo = 3;

        //Semaphore[] Garfo = new Semaphore[5];
        private void Form_Principal_Load(object sender, EventArgs e)
        {
            Semaforo = new Semaphore(2, 2);
            F1 = new Thread(Filosofo1);
            F2 = new Thread(Filosofo2);
            F3 = new Thread(Filosofo3);
            F4 = new Thread(Filosofo4);
            F5 = new Thread(Filosofo5);

            F1.IsBackground = false;
            F2.IsBackground = false;
            F3.IsBackground = false;
            F4.IsBackground = false;
            F5.IsBackground = false;

            for(int i=0;i<5;i++)
            {
                Filosofos[i] = new Filosofo();
                Filosofos[i].Texto = "Filosofo " + (i + 1).ToString();
            }

            for (int i = 0; i < 5; i++)
            {
                Garfo[i] = new Garfo();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            count_f1.Text = "0";
            count_f2.Text = "0";
            count_f3.Text = "0";
            count_f4.Text = "0";
            count_f5.Text = "0";

            for (int i = 0; i < Garfo.Count(); i++)
            {
                Garfo[i].EstaEmUso = false;
                Filosofos[i].EstaPensando = false;
            }

            controlaThreads = true;

            F1.Abort();
            F2.Abort();
            F3.Abort();
            F4.Abort();
            F5.Abort();

            { F1 = new Thread(Filosofo1); F1.Start(); }
            { F3 = new Thread(Filosofo3); F3.Start(); }
            { F5 = new Thread(Filosofo5); F5.Start(); }
            { F2 = new Thread(Filosofo2); F2.Start(); }
            { F4 = new Thread(Filosofo4); F4.Start(); }
            button1.Enabled = false;
            button2.Enabled = true;
        }


        Random rand = new Random();
        private void Filosofo1()
        {
            while (controlaThreads)
            {
                if (Garfo[0].EstaEmUso || Garfo[4].EstaEmUso)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        prato_Filosofo1.Image = Properties.Resources.Prato_Amarelo;
                        listBox1.Items.Add(Filosofos[0].Texto + "está pensando. Garfo ja esta em uso." + ((Garfo[0].EstaEmUso == true) ? "  - Garfo 1 - " : " ") + ((Garfo[4].EstaEmUso == true) ? " Garfo 5" : " "));
                        status_f1.Text = "AGUARDANDO";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                    Thread.Sleep(2000);
                }
                else
                {
                    if (Garfo[0].EstaEmUso || Garfo[4].EstaEmUso)
                        return;

                    int tempo = rand.Next(3000, 6000);
                    int tempoComendo = tempo / controlaTempo;

                    //INICIO DO BLOCO COMENDO
                    Semaforo.WaitOne();
                    {
                        Filosofos[0].EstaPensando = false;//PARA DE PENSAR
                        Garfo[0].EstaEmUso = true;//PEGA O GARFO 1
                        Garfo[4].EstaEmUso = true;//PEGA O GARFO 5
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo1.Image = Properties.Resources.Prato_verde;
                            prato_Filosofo1.Refresh();
                            garfo1.Visible = false;
                            garfo5.Visible = false;
                            listBox1.Items.Add(Filosofos[0].Texto + " está COMENDO");
                            status_f1.Text = "COMENDO";

                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            count_f1.Text = (int.Parse(count_f1.Text) + 1).ToString();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo1.Image = Properties.Resources.Prato_verdeMetade;
                            prato_Filosofo1.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo1.Image = Properties.Resources.pratov;
                            prato_Filosofo1.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo1.Image = Properties.Resources.Prato_vermelho;
                            garfo1.Visible = true;
                            garfo5.Visible = true;
                            listBox1.Items.Add(Filosofos[0].Texto + " está pensando");
                            status_f1.Text = "PENSANDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                        Garfo[0].EstaEmUso = false;//DEIXA O GARFO 1
                        Garfo[4].EstaEmUso = false;//DEIXA O GARFO 5
                        Filosofos[0].EstaPensando = true;//VOLTA A PENSAR
                    }
                    Semaforo.Release();
                    //FIM DO BLOCO COMENDO                    

                    tempo = rand.Next(1000, 3000);
                    Thread.Sleep(tempo);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            controlaThreads = false;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Filosofo2()
        {
            while (controlaThreads)
            {
                if (Garfo[0].EstaEmUso || Garfo[1].EstaEmUso)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        prato_Filosofo2.Image = Properties.Resources.Prato_Amarelo;
                        listBox1.Items.Add(Filosofos[1].Texto + "está pensando. Garfo ja esta em uso." + ((Garfo[0].EstaEmUso == true) ? "  - Garfo 1 - " : " ") + ((Garfo[1].EstaEmUso == true) ? " Garfo 2" : " "));
                        status_f2.Text = "AGUARDANDO";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                    Thread.Sleep(2000);
                }
                else
                {
                    if (Garfo[0].EstaEmUso || Garfo[1].EstaEmUso)
                        return;

                    int tempo = rand.Next(3000, 6000);
                    int tempoComendo = tempo / controlaTempo;

                    //INICIO DO BLOCO COMENDO
                    Semaforo.WaitOne();
                    {
                        Filosofos[1].EstaPensando = false;//PARA DE PENSAR
                        Garfo[0].EstaEmUso = true;//PEGA O GARFO 1
                        Garfo[1].EstaEmUso = true;//PEGA O GARFO 2
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo2.Image = Properties.Resources.Prato_verde;
                            garfo1.Visible = false;
                            garfo2.Visible = false;
                            listBox1.Items.Add(Filosofos[1].Texto + " está COMENDO");
                            status_f2.Text = "COMENDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            count_f2.Text = (int.Parse(count_f2.Text) + 1).ToString();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo2.Image = Properties.Resources.Prato_verdeMetade;
                            prato_Filosofo2.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo2.Image = Properties.Resources.pratov;
                            prato_Filosofo2.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo2.Image = Properties.Resources.Prato_vermelho;
                            garfo1.Visible = true;
                            garfo2.Visible = true;
                            listBox1.Items.Add(Filosofos[1].Texto + " está pensando");
                            status_f2.Text = "PENSANDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                        Garfo[0].EstaEmUso = false;//DEIXA O GARFO 1
                        Garfo[1].EstaEmUso = false;//DEIXA O GARFO 2
                        Filosofos[1].EstaPensando = true;//VOLTA A PENSAR
                    }
                    Semaforo.Release();
                    //FIM DO BLOCO COMENDO

                    tempo = rand.Next(1000, 3000);
                    Thread.Sleep(tempo);
                }
            }
        }
        private void Filosofo3()
        {
            while (controlaThreads)
            {
                if (Garfo[1].EstaEmUso || Garfo[2].EstaEmUso)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        prato_Filosofo3.Image = Properties.Resources.Prato_Amarelo;
                        listBox1.Items.Add(Filosofos[2].Texto + "está pensando. Garfo ja esta em uso." + ((Garfo[1].EstaEmUso == true) ? "  - Garfo 2 - " : " ") + ((Garfo[2].EstaEmUso == true) ? " Garfo 3" : " "));
                        status_f3.Text = "AGUARDANDO";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                    Thread.Sleep(2000);
                }
                else
                {
                    if (Garfo[1].EstaEmUso || Garfo[2].EstaEmUso)
                        return;

                    int tempo = rand.Next(3000, 6000);
                    int tempoComendo = tempo / controlaTempo;

                    //INICIO DO BLOCO COMENDO
                    Semaforo.WaitOne();
                    {
                        Filosofos[2].EstaPensando = false;//PARA DE PENSAR
                        Garfo[1].EstaEmUso = true;//PEGA GARFO 2
                        Garfo[2].EstaEmUso = true;//PEGA GARFO 3
                        this.BeginInvoke((MethodInvoker)delegate
                            {
                                prato_Filosofo3.Image = Properties.Resources.Prato_verde;
                                garfo2.Visible = false;
                                garfo3.Visible = false;
                                listBox1.Items.Add(Filosofos[2].Texto + " está COMENDO");
                                status_f3.Text = "COMENDO";
                                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                                count_f3.Text = (int.Parse(count_f3.Text) + 1).ToString();
                            });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo3.Image = Properties.Resources.Prato_verdeMetade;
                            prato_Filosofo3.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo3.Image = Properties.Resources.pratov;
                            prato_Filosofo3.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo3.Image = Properties.Resources.Prato_vermelho;
                            garfo2.Visible = true;
                            garfo3.Visible = true;
                            listBox1.Items.Add(Filosofos[2].Texto + " está pensando");
                            status_f3.Text = "PENSANDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                        Garfo[1].EstaEmUso = false;//DEIXA GARFO 2
                        Garfo[2].EstaEmUso = false;//DEIXA GARFO 3
                        Filosofos[2].EstaPensando = true;//VOLTA A PENSAR
                    }
                    Semaforo.Release();
                    //FIM DO BLOCO COMENDO

                    tempo = rand.Next(1000, 3000);
                    Thread.Sleep(tempo);
                }
            }
        }
        private void Filosofo4()
        {
            while (controlaThreads)
            {
                if (Garfo[2].EstaEmUso || Garfo[3].EstaEmUso)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        prato_Filosofo4.Image = Properties.Resources.Prato_Amarelo;
                        listBox1.Items.Add(Filosofos[3].Texto + "está pensando. Garfo ja esta em uso." + ((Garfo[2].EstaEmUso == true) ? "  - Garfo 3 - " : " ") + ((Garfo[3].EstaEmUso == true) ? " Garfo 4" : " "));
                        status_f4.Text = "AGUARDANDO";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                    Thread.Sleep(2000);
                }
                else
                {
                    if (Garfo[2].EstaEmUso || Garfo[3].EstaEmUso)
                        return;

                    int tempo = rand.Next(3000, 6000);
                    int tempoComendo = tempo / controlaTempo;

                    //INICIO DO BLOCO COMENDO
                    Semaforo.WaitOne();
                    {
                        Filosofos[3].EstaPensando = false;//PARA DE PENSAR
                        Garfo[2].EstaEmUso = true;//PEGA GARFO 3
                        Garfo[3].EstaEmUso = true;//PEGA GARFO 4
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo4.Image = Properties.Resources.Prato_verde;
                            garfo3.Visible = false;
                            garfo4.Visible = false;
                            listBox1.Items.Add(Filosofos[3].Texto + " está COMENDO");
                            status_f4.Text = "COMENDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            count_f4.Text = (int.Parse(count_f4.Text) + 1).ToString();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo4.Image = Properties.Resources.Prato_verdeMetade;
                            prato_Filosofo4.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo4.Image = Properties.Resources.pratov;
                            prato_Filosofo4.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo4.Image = Properties.Resources.Prato_vermelho;
                            garfo3.Visible = true;
                            garfo4.Visible = true;
                            listBox1.Items.Add(Filosofos[3].Texto + " está pensando");
                            status_f4.Text = "PENSANDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                        Garfo[2].EstaEmUso = false;//DEIXA GARFO 3
                        Garfo[3].EstaEmUso = false;//DEIXA GARFO 4
                        Filosofos[3].EstaPensando = true;//VOLTA A PENSAR
                    }
                    Semaforo.Release();
                    //FIM DO BLOCO COMENDO

                    tempo = rand.Next(1000, 3000);
                    Thread.Sleep(tempo);
                }
            }
        }
        private void Filosofo5()
        {
            while (controlaThreads)
            {
                if (Garfo[3].EstaEmUso || Garfo[4].EstaEmUso)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        prato_Filosofo5.Image = Properties.Resources.Prato_Amarelo;
                        listBox1.Items.Add(Filosofos[4].Texto + "está pensando. Algum garfo ja esta em uso" + ((Garfo[3].EstaEmUso == true) ? "  - Garfo 4 - " : " ") + ((Garfo[4].EstaEmUso == true) ? " Garfo 5" : " "));
                        status_f5.Text = "AGUARDANDO";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                    Thread.Sleep(2000);
                }
                else
                {
                    if (Garfo[3].EstaEmUso || Garfo[4].EstaEmUso)
                        return;

                    int tempo = rand.Next(3000, 6000);
                    int tempoComendo = tempo / controlaTempo;

                    //INICIO BLOCO COMENDO
                    Semaforo.WaitOne();
                    {
                        Filosofos[4].EstaPensando = false;//PARA DE PENSAR
                        Garfo[3].EstaEmUso = true;//PEGA GARFO 4
                        Garfo[4].EstaEmUso = true;//PEGA GARFO 5
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo5.Image = Properties.Resources.Prato_verde;
                            garfo4.Visible = false;
                            garfo5.Visible = false;
                            listBox1.Items.Add(Filosofos[4].Texto + " está COMENDO");
                            status_f5.Text = "COMENDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            count_f5.Text = (int.Parse(count_f5.Text) + 1).ToString();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo5.Image = Properties.Resources.Prato_verdeMetade;
                            prato_Filosofo5.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo5.Image = Properties.Resources.pratov;
                            prato_Filosofo5.Refresh();
                        });
                        Thread.Sleep(tempoComendo);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            prato_Filosofo5.Image = Properties.Resources.Prato_vermelho;
                            garfo4.Visible = true;
                            garfo5.Visible = true;
                            listBox1.Items.Add(Filosofos[4].Texto + " está pensando");
                            status_f5.Text = "PENSANDO";
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                        Garfo[3].EstaEmUso = false;//DEIXA GARFO 4
                        Garfo[4].EstaEmUso = false;//DEIXA GARGO 5
                        Filosofos[4].EstaPensando = true;//VOLTA A PENSAR
                    }
                    Semaforo.Release();
                    //FIM DO BLOCO COMENDO

                    tempo = rand.Next(1000, 3000);
                    Thread.Sleep(tempo);
                }
            }
        }
    }
}
