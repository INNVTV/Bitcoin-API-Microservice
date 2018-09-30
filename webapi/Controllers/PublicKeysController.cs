using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;

namespace webapi.Controllers
{
    [Route("api/generate/[controller]")]
    [ApiController]
    public class PublicKeysController : ControllerBase
    {
        // GET api/generate/publickeys/1
        [HttpGet("{count}")]
        public ActionResult<List<string>> Get(int count)
        {
            var pubKeysReturnList = new List<string>();

            for(int i =0; i < count; i++)
            {
                //Generate key
                var key = new Key().GetWif(Network.Main);

                //Store pubKey into return list
                pubKeysReturnList.Add(key.GetAddress().ToString()); //<=== key.GetSegwitAddress() //<=== [If you prefer Segwit]

                //Store record in MongoDB:
                var client = new MongoClient(Program._mongoUri);

                //Pick up all latest new registrations
                if(client != null)
                {
                    var _database = client.GetDatabase(Program._mongoDbName);
                    var keysCollection =_database.GetCollection<BsonDocument>("keys");

                    keysCollection.InsertOne(
                        new BsonDocument{
                            {"public", key.GetAddress().ToString()},
                            {"private", key.ToWif()}
                        });
                }
            }

            //Return public keys
            return  pubKeysReturnList;
        }
    }
}
