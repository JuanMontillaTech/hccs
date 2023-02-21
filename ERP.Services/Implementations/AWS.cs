using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ERP.Domain.Command;
using ERP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Implementations
{
    public class AWS : IAWS
    {
        // note: this constructor relies on you having set up a credential profile
        // named 'default', or have set credentials in environment variables
        // AWS_ACCESS_KEY_ID & AWS_SECRET_ACCESS_KEY, or have an application settings
        // file. See https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
        // for more details and other constructor options.
        // bucket names in Amazon S3 must be globally unique and lowercase
      
        public string key = $"-{Guid.NewGuid().ToString("n").Substring(0, 8)}";
        private readonly ICurrentUser _getCurrentUser;
        private const string _awsAccessKeyId = "AKIAXWXHIPJOV7TK27VE";
        private const string _awsSecretAccessKey = "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";

        public AWS(ICurrentUser getCurrentUser)
        {
            _getCurrentUser = getCurrentUser;
        }

        public bool CreateBucket(string bucketName)
        {
            using (var s3 = new AmazonS3Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.USEast1))
            {
                var req = new PutBucketRequest
                {
                    BucketName = bucketName,
                    BucketRegion = S3Region.USEast1 
                };

                Task<PutBucketResponse> res = s3.PutBucketAsync(req);

                Task.WaitAll(res);

                if (res.IsCompletedSuccessfully)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Result<string> WriteObject(MemoryStream ms, string bucketName, string fileName)
        {
            // The api call used in this method equates to S3's Put api and is
            // suitable for smaller files. To upload largker files and entire
            // suitable for smaller files. To upload largker files and entire
            // folder hierarchies, with automatic usage of S3's multi-part apis for
            // files over 5MB in size, consider using the TransferUtility class
            // in the Amazon.S3.TransferEE. UU. Este (Norte de Virginia) us-east-1 namespace.
            // See https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html.
            bucketName = "montillasoft." + _getCurrentUser.DataBaseName();
          
 

            try
            {
                using (var s3 = new AmazonS3Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.USEast1))
                {
                    var req = new PutObjectRequest
                    {
                        BucketName = bucketName.ToLower(),
                        Key = fileName,
                        InputStream = ms
                    };

                    Task<PutObjectResponse> res = s3.PutObjectAsync(req);

                    Task.WaitAll(res);

                    if (res.IsCompletedSuccessfully)
                    {
                        return Result<string>.Success(fileName, "Documento Guardado" + fileName);
                    }
                    else
                    {
                        return Result<string>.Fail("Error creando", fileName);
                    }
                }
            }
            catch (Exception ex)
            {

                return Result<string>.Fail("Error creando", ex.Message);
            }
        }

        public Task<GetObjectResponse> ReadObject(string bucketName)
        {
            // The api call used in this method equates to S3's Get api and is
            // suitable for smaller files. To download larger files, with
            // automatic usage of S3's multi-part apis for files over 5MB in size,
            // consider using the TransferUtility class in the Amazon.S3.Transfer
            // namespace.
            // See https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html.

            using (var s3 = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                var req = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                Task<GetObjectResponse> res = s3.GetObjectAsync(req);
                Task.WaitAll(res);

                if (res.IsCompletedSuccessfully)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<S3Bucket> ListBuckets()
        {
            using (var s3 = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                // listing buckets takes no request parameters, so a
                // parameterless override is used here instead of creating
                // a ListBucketsRequest object.

                Task<ListBucketsResponse> res = s3.ListBucketsAsync();
                Task.WaitAll(res);

                return res.Result.Buckets;
            }
        }

        public List<S3Object> ListObjects(string bucketName)
        {
            using (var s3 = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                var req = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = 100
                };

                Task<ListObjectsResponse> res = s3.ListObjectsAsync(req);
                Task.WaitAll(res);

                Console.WriteLine("List of objects in your S3 Bucket '{0}'", bucketName);
                foreach (var s3Object in res.Result.S3Objects)
                {
                    Console.WriteLine(s3Object.Key);
                }
                return res.Result.S3Objects;
            }
        }

        public bool DeleteObject(string bucketName, string fileName)
        {
            using (var s3 = new AmazonS3Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.USEast1))
            {
                var req = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                Task<DeleteObjectResponse> res = s3.DeleteObjectAsync(req);
                Task.WaitAll(res);

                if (res.IsCompletedSuccessfully)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteBucket(string bucketName)
        {
            using (var s3 = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                // S3 requires that buckets are empty of objects before they can
                // be deleted. The SDK also contains a helper utility, AmazonS3Util,
                // in the Amazon.S3.Util namespace with various methods that allow
                // you to delete non-empty buckets.

                var req = new DeleteBucketRequest
                {
                    BucketName = bucketName
                };

                Task<DeleteBucketResponse> res = s3.DeleteBucketAsync(req);
                Task.WaitAll(res);

                if (res.IsCompletedSuccessfully)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
