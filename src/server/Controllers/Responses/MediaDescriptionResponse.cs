﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers.Responses
{
    public abstract class XmlResponse : IActionResult
    {
        protected string BaseXmlTemplate { get;  }
        public abstract string GetXml();

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            var xml = GetXml();
            response.ContentLength = xml.Length;
            using (var stream = StreamService.FromString(xml))
            {

                await stream.CopyToAsync(context.HttpContext.Response.Body);
            }
        }
    }

    public class MediaDescriptionResponse : XmlResponse
    {
        public string IpAddress { get; set; }
        public string Uuid { get; set; }
        public string SerialNumber { get; set; }

        public string FriendlyName { get; set; }

        public string BaseXmlTemplate = @"
<root xmlns = ""urn:schemas-upnp-org:device-1-0"" xmlns:yamaha=""urn:schemas-yamaha-com:device-1-0"">
   <specVersion>
      <major>1</major>
      <minor>0</minor>
   </specVersion>
   <device>
      <dlna:X_DLNADOC xmlns:dlna=""urn:schemas-dlna-org:device-1-0"">DMR-1.50</dlna:X_DLNADOC>
      <deviceType>urn:schemas-upnp-org:device:MediaRenderer:1</deviceType>
      <friendlyName>!!FRIENDLY_NAME!!</friendlyName>
      <manufacturer>Yamaha Corporation</manufacturer>
      <manufacturerURL>http://www.yamaha.com/</manufacturerURL>
      <modelDescription>MusicCast</modelDescription>
      <modelName>WX-030</modelName>
      <modelNumber>030</modelNumber>
      <modelURL>http://www.yamaha.com/</modelURL>
      <serialNumber>!!SERIAL_NUMBER!!</serialNumber>
      <UDN>!!UUID!!</UDN>
      <iconList>
         <icon>
            <mimetype>image/jpeg</mimetype>
            <width>48</width>
            <height>48</height>
            <depth>24</depth>
            <url>/Icons/48x48.jpg</url>
         </icon>
         <icon>
            <mimetype>image/jpeg</mimetype>
            <width>120</width>
            <height>120</height>
            <depth>24</depth>
            <url>/Icons/120x120.jpg</url>
         </icon>
         <icon>
            <mimetype>image/png</mimetype>
            <width>48</width>
            <height>48</height>
            <depth>24</depth>
            <url>/Icons/48x48.png</url>
         </icon>
         <icon>
            <mimetype>image/png</mimetype>
            <width>120</width>
            <height>120</height>
            <depth>24</depth>
            <url>/Icons/120x120.png</url>
         </icon>
      </iconList>
      <serviceList>
         <service>
            <serviceType>urn:schemas-upnp-org:service:AVTransport:1</serviceType>
            <serviceId>urn:upnp-org:serviceId:AVTransport</serviceId>
            <SCPDURL>/AVTransport/desc.xml</SCPDURL>
            <controlURL>/AVTransport/ctrl</controlURL>
            <eventSubURL>/AVTransport/event</eventSubURL>
         </service>
         <service>
            <serviceType>urn:schemas-upnp-org:service:RenderingControl:1</serviceType>
            <serviceId>urn:upnp-org:serviceId:RenderingControl</serviceId>
            <SCPDURL>/RenderingControl/desc.xml</SCPDURL>
            <controlURL>/RenderingControl/ctrl</controlURL>
            <eventSubURL>/RenderingControl/event</eventSubURL>
         </service>
         <service>
            <serviceType>urn:schemas-upnp-org:service:ConnectionManager:1</serviceType>
            <serviceId>urn:upnp-org:serviceId:ConnectionManager</serviceId>
            <SCPDURL>/ConnectionManager/desc.xml</SCPDURL>
            <controlURL>/ConnectionManager/ctrl</controlURL>
            <eventSubURL>/ConnectionManager/event</eventSubURL>
         </service>
      </serviceList>
      <presentationURL>http://!!IP_ADDRESS!!/</presentationURL>
   </device>
   <yamaha:X_device>
      <yamaha:X_URLBase>http://!!IP_ADDRESS!!:80/</yamaha:X_URLBase>
      <yamaha:X_serviceList>
         <yamaha:X_service>
            <yamaha:X_specType>urn:schemas-yamaha-com:service:X_YamahaRemoteControl:1</yamaha:X_specType>
            <yamaha:X_controlURL>/YamahaRemoteControl/ctrl</yamaha:X_controlURL>
            <yamaha:X_unitDescURL>/YamahaRemoteControl/desc.xml</yamaha:X_unitDescURL>
         </yamaha:X_service>
         <yamaha:X_service>
            <yamaha:X_specType>urn:schemas-yamaha-com:service:X_YamahaExtendedControl:1</yamaha:X_specType>
            <yamaha:X_yxcControlURL>/YamahaExtendedControl/v1/</yamaha:X_yxcControlURL>
            <yamaha:X_yxcVersion>0516</yamaha:X_yxcVersion>
         </yamaha:X_service>
      </yamaha:X_serviceList>
   </yamaha:X_device>
</root>
";
        public override string GetXml()
        {
            var output = BaseXmlTemplate
                .Replace("!!IP_ADDRESS!!", IpAddress)
                .Replace("!!SERIAL_NUMBER!!", SerialNumber)
                .Replace("!!FRIENDLY_NAME!!", FriendlyName)
                .Replace("!!UUID!!", Uuid);

            return output;
        }
    }
}