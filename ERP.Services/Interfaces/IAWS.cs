using Amazon.S3.Model;
using ERP.Domain.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Interfaces
{
    public interface IAWS
    {
        bool CreateBucket(string bucketName);
        
        Result<string> WriteObject(MemoryStream ms, string bucketName, string fileName);
        Task<GetObjectResponse> ReadObject(string bucketName);
        List<S3Bucket> ListBuckets();
        List<S3Object> ListObjects(string bucketName);
        bool DeleteObject(string bucketName, string fileName);
        bool DeleteBucket(string bucketName);
    }
}
