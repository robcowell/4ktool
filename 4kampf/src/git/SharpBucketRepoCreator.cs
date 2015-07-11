using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBucket.V2.Pocos;
using SharpBucket;
using RestSharp;
using SharpBucket.Authentication;
using System.Net;

namespace SharpBucket.V2.EndPoints {
    class SharpBucketRepoCreator : RepositoriesEndPoint {

        internal SharpBucketV2 _sharpBucketV2;
        internal string _baseUrl;

        private Authenticate authenticator;

        public SharpBucketRepoCreator(SharpBucketV2 sharpBucketV2) : base(sharpBucketV2){
            _sharpBucketV2 = sharpBucketV2;
            _baseUrl = "repositories/";
        }

        public Pocos.Repository CreateRepository(Repository repository, string team) {
            return this.PostRepository(repository, team);
        }

        internal Repository PostRepository(Repository repo, string accountName) {
            var overrideUrl = GetRepositoryUrl(accountName, repo.name, null);
            return this.Post(repo, overrideUrl);
        }

        internal string GetRepositoryUrl(string accountName, string repository, string append) {
            var format = _baseUrl + "{0}/{1}/{2}";
            return string.Format(format, accountName, repository, append);
        }


        internal T Get<T>(T body, string overrideUrl) {
            return Send(body, Method.GET, overrideUrl);
        }

        internal T Post<T>(T body, string overrideUrl) {
            return Send(body, Method.POST, overrideUrl);
        }

        internal T Put<T>(T body, string overrideUrl) {
            return Send(body, Method.PUT, overrideUrl);
        }

        internal T Delete<T>(T body, string overrideUrl) {
            return Send(body, Method.DELETE, overrideUrl);
        }


        private T Send<T>(T body, Method method, string overrideUrl = null) {
            var relativeUrl = overrideUrl;
            T response;
            try {
                response = authenticator.GetResponse(relativeUrl, method, body);
            } catch (WebException ex) {
                Console.WriteLine(ex.Message);
                response = default(T);
            }
            return response;
        }

        public void BasicAuthentication(string username, string password) {
            authenticator = new BasicAuthentication(username, password, _baseUrl);
        }
    }
}
