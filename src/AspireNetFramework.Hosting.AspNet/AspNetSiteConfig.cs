using System.Xml.Serialization;

namespace AspireNetFramework.Hosting.AspNet;

[XmlRoot("configuration")]
public class ApplicationHostConfiguration
{
    [XmlElement("system.applicationHost")]
    public required SystemApplicationHost SystemApplicationHost { get; set; }
}

public class SystemApplicationHost
{
    [XmlArray("sites")]
    [XmlArrayItem("site", typeof(Site))]
    public required Site[] Sites { get; set; }
}

public class Site
{
    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlElement("application")]
    public required Application Application { get; set; }

    [XmlArray("bindings")]
    [XmlArrayItem("binding", typeof(Binding))]
    public required Binding[] Bindings { get; set; }
}

public class Application
{
    [XmlElement("virtualDirectory")]
    public required VirtualDirectory VirtualDirectory { get; set; }
}

public class VirtualDirectory
{
    [XmlAttribute("physicalPath")]
    public required string PhysicalPath { get; set; }
}

public class Binding
{
    [XmlAttribute("protocol")]
    public required string Protocol { get; set; }

    [XmlAttribute("bindingInformation")]
    public required string BindingInformation { get; set; }

    public int Port => int.Parse(BindingInformation.Split(':')[1]);
}
