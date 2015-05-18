using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DeployListener.DeployCommands;

namespace DeployListener.Controllers
{
    public class DeploymentController : ApiController
    {
        public HttpResponseMessage Post()
        {
            string requestBackupPath = Path.Combine(Environment.CurrentDirectory, "RequestZipFiles");
            Console.WriteLine(requestBackupPath);

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(requestBackupPath);

            return HttpContentMultipartExtensions.ReadAsMultipartAsync<MultipartFormDataStreamProvider>(this.Request.Content, streamProvider)
                .ContinueWith<HttpResponseMessage>((Func<Task<MultipartFormDataStreamProvider>, HttpResponseMessage>)(task =>
                {
                    //if any internal server error occurs during file read server error response will be sent
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, (Exception)task.Exception);
                    }

                    var sourcePath =
                        Enumerable.FirstOrDefault<string>(
                            Enumerable.Select((IEnumerable<MultipartFileData>)streamProvider.FileData,
                                              (p => p.LocalFileName)));

                    var deployRequestHandler = new DeployRequestHandler();
                    deployRequestHandler.Handle(new DeployRequestDto
                    {
                        SourcePath = sourcePath,
                        DeployPath = "C:\\Test"
                    });
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Deployment is successfull");
                })).Result;
            return this.Request.CreateResponse(HttpStatusCode.Created,"ok");
        }
    }
}