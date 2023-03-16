using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommManagement
{
    public class FileManagement
    {
        
        public async Task<string> UploadFileToS3(IFormFile uploadedfile, string filename, string bucketname, string accesskey, string secretkey)
        {
            using (var client = new AmazonS3Client(accesskey, secretkey, RegionEndpoint.APSouth1))
            {

                using (var newMemoryStream = new MemoryStream())
                {
                    uploadedfile.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = filename,
                        BucketName = bucketname
                        //CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    //Amazon.S3.Model.GetObjectRequest g = new();
                    //g.BucketName = bucketname;
                    //g.Key = filename;


                    //var chk = await client.GetObjectAsync(g);

                    ////temporary. Needs to be removed
                    //Amazon.S3.Model.GetPreSignedUrlRequest req = new();

                    //req.BucketName = bucketname;
                    //req.Key = filename;
                    //req.Expires = DateTime.Now.AddDays(6);
                    ////temporary. Needs to be removed

                    //return (client.GetPreSignedURL(req));

                    return $"http://{bucketname}.s3-ap-south-1.amazonaws.com/{filename}";

                    //http://ecommuploads.s3-ap-south-1.amazonaws.com/
                }
            }
        }
    }
}
