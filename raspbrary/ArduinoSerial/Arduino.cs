using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace ArduinoSerial
{
    public class Arduino
    {
        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;
        private ObservableCollection<DeviceInformation> listOfDevices;
        public CancellationTokenSource ReadCancellationTokenSource;
        private Action<string> onRe;
        public async Task connect()
        {
            if (serialPort == null)
            {
                listOfDevices = new ObservableCollection<DeviceInformation>();
                ReadCancellationTokenSource = new CancellationTokenSource();
                DeviceInformationCollection dis;
                try
                {
                    string aqs = SerialDevice.GetDeviceSelector();
                    dis = await DeviceInformation.FindAllAsync(aqs);
                    DeviceInformation entry = dis[0];
                    serialPort = await SerialDevice.FromIdAsync(entry.Id);
                    serialPort.WriteTimeout = TimeSpan.FromMilliseconds(2000);
                    serialPort.ReadTimeout = TimeSpan.FromMilliseconds(2000);
                    serialPort.BaudRate = 9600;
                    serialPort.Parity = SerialParity.None;
                    serialPort.StopBits = SerialStopBitCount.One;
                    serialPort.DataBits = 8;
                }
                catch(Exception ex)
                {
                }
            }
        }

        private async void Listen(Action<string> onReceive)
        {
            while (true)
            {
                try
                {
                    if (serialPort != null)
                    {
                        dataReaderObject = new DataReader(serialPort.InputStream);
                        while (true)
                        {
                            await ReadAsync(ReadCancellationTokenSource.Token);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CloseDevice();
                }
                finally
                {
                    if (dataReaderObject != null)
                    {
                        dataReaderObject.DetachStream();
                        dataReaderObject = null;
                    }
                }
            }
        }

        private void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;
            listOfDevices.Clear();
        }

        public async Task WriteAsync(string data)
        {
            DataWriter serialDataWriter = new DataWriter(serialPort.OutputStream);
            Task<uint> storeAsyncTask;
            serialDataWriter.WriteString(data);
            storeAsyncTask = serialDataWriter.StoreAsync().AsTask();
            uint bytesWritten = await storeAsyncTask;
            serialDataWriter.DetachStream();
        }

        public async Task<string> ReadAsync(CancellationToken cancellationToken)
        {
            dataReaderObject = new DataReader(serialPort.InputStream);
            Task<uint> loadAsyncTask;
            uint ReadBufferLength = 1024;
            cancellationToken.ThrowIfCancellationRequested();
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            // Launch the task and wait
            uint bytesRead = await loadAsyncTask;
            while (true)
            {
                if (bytesRead > 0)
                {
                    string result = dataReaderObject.ReadString(bytesRead);
                    dataReaderObject.DetachStream();
                    return result;
                }
            }
            
        }
    }
}
