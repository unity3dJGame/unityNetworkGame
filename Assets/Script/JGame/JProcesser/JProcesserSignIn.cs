using System;
using System.Net;
using System.Net.Sockets;

namespace JGame
{
	using StreamObject;
	using Network;
	using Data;

	namespace Processer
	{

		public static class JSocket
		{
			public static string _server_ip = "127.0.0.1";
			public static int	  _server_port = 9796;	
			public static IPEndPoint server_edp;
			public static Socket Client_socket;

			public static void initialize()
			{
				IPAddress ip_server = IPAddress.Parse (_server_ip); 
				server_edp = new IPEndPoint (ip_server, _server_port);
				Client_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				//尝试连接服务器
				try
				{
					JSocket.Client_socket.Connect(JSocket.server_edp);
					JLog.Debug("连接服务器成功");
					return ;
				}
				catch (Exception e) {
					JLog.Debug("连接服务器失败");
					JLog.Error (e.Message);
					return;
				}
			}


		}

		public class JProcesserSignIn : IProcesser
		{

			protected JProcesserSignIn() { JSocket.initialize(); }

			public virtual void run(IDataSet dataSet)
			{
			}

			#region 私有方法

			protected bool SendToServer(JObj_SignIn obj)
			{
				try {
					JOutputStream jstream = new JOutputStream ();
					JBinaryReaderWriter.Write (ref jstream, obj);
					JSocket.Client_socket.Send (jstream.ToArray ());
					return true;
				} catch (Exception e) {
					JLog.Debug ("发送数据失败");
					JLog.Error (e.Message);
					return false;
				}
			}

			protected IStreamObj getObjFromServer()
			{
				int nReceivedCount = 0;
				byte[] buffer = new byte[JTcpDefines.max_buffer_size];
				int nCount = 0;
				do {
					nReceivedCount = JSocket.Client_socket.Receive (buffer, JTcpDefines.max_buffer_size, SocketFlags.None);
					nCount++;
				} while(nReceivedCount == 0 && nCount < 5);

				//...received packet
				JInputStream inputStream = new JInputStream(buffer);
				IStreamObj receivedObj = JBinaryReaderWriter.Read<IStreamObj>(inputStream);
				//ushort utype = receivedObj.Type();
				return receivedObj;
			}
			#endregion
		}

		public class JProcesserSignInSet :  JProcesserSignIn
		{
			public override void run(IDataSet dataSet)
			{
				IStreamObj obj = dataSet.getData (JObjectType.sign_in);
				JObj_SignIn signInObj = obj as JObj_SignIn;
				if (signInObj == null)
					return;
				if (JSocket.Client_socket.Connected)
					SendToServer (signInObj);
			}
		}

		public class JProcesserSignInGet: JProcesserSignIn
		{
			public override void run(IDataSet dataSet)
			{
				if (JSocket.Client_socket.Connected) {
					IStreamObj obj = getObjFromServer ();
					dataSet.setData (obj);
				}
			}
		}
	}
}

