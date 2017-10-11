using System.IO;
using NUnit.Framework;

namespace roundhouse.tests.infrastructure.filesystem
{
    using roundhouse.consoles;
    using roundhouse.infrastructure.filesystem;
    using System.Text;

    public class DotNetFileSystemAccessSpecs
    {
        public abstract class concern_for_file_system : TinySpec<DotNetFileSystemAccess>
        {
            protected object result;
            private DotNetFileSystemAccess dot_net_file_system_access = new DotNetFileSystemAccess(new DefaultConfiguration());

            public override void Context() { }
            protected override DotNetFileSystemAccess sut
            {
                get { return dot_net_file_system_access; }
                set { dot_net_file_system_access = value; }
            }
        }

        [Concern(typeof(DotNetFileSystemAccess))]
        public class when_reading_files_with_different_formats : concern_for_file_system
        {
            protected static string utf8_file;
            protected static string ansi_file;

            public override void Because()
            {
                utf8_file = sut.read_file_text(Path.Combine(TestContext.CurrentContext.TestDirectory, @"infrastructure\filesystem\utf8encoded.txt"));
                ansi_file = sut.read_file_text(Path.Combine(TestContext.CurrentContext.TestDirectory, @"infrastructure\filesystem\ansiencoded.txt"));
            }

            [Observation]
            public void utf8_encoded_file_should_read_correctly()
            {
                utf8_file.should_be_equal_to("INSERT INTO [dbo].[timmy]([value]) VALUES('G�')");
            }

            [Observation]
            public void ansi_encoded_file_should_read_correctly()
            {
                ansi_file.should_be_equal_to(to_default_code_page("INSERT INTO [dbo].[timmy]([value]) VALUES('G�')"));
            }

            private string to_default_code_page(string english_code_page_text)
            {
                var bytes = Encoding.GetEncoding(1252).GetBytes(english_code_page_text);
                return Encoding.Default.GetString(bytes);
            }
        }
    }
}