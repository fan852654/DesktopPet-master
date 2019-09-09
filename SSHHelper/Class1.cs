using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHHelper
{
    public class SSHHelper
    {
        public SSHHelper()
        {
            ConnectionInfo connectionInfo = new ConnectionInfo("ru.strygwyr.top", "root", new PasswordAuthenticationMethod("root", "fanzs900839"));
            SshClient ssh = new SshClient(connectionInfo);
            var stre = ssh.CreateShellStream("a", 0, 0, 0, 0, 1024);
        }
    }
}
