//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.OnePay
{
    using Microsoft.OnePay.Models.Storage;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    // TODO: If you have time make all of this async
    public static class DataAccess
    {
        private const string AccountContainerName = "accounts";
        private const string TransactionContainerName = "transactions";

        private static CloudStorageAccount StorageAccount { get; set; }

        private static CloudBlobClient BlobClient { get; set; }

        static DataAccess()
        {
            StorageCredentials credentials = new StorageCredentials(StorageConstants.StorageAccount, StorageConstants.StorageKey);
            StorageAccount = new CloudStorageAccount(credentials, true);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public static Transaction GetTransaction(string paymentId)
        {
            Transaction transaction = null;

            CloudBlobContainer container = BlobClient.GetContainerReference(TransactionContainerName);
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(paymentId);
            if (blockBlob.Exists())
            {
                var document = blockBlob.DownloadText();
                transaction = JsonConvert.DeserializeObject<Transaction>(document);
            }

            return transaction;
        }

        internal static IEnumerable<Transaction> GetTransactions()
        {
            Transaction transaction = null;

            CloudBlobContainer container = BlobClient.GetContainerReference(TransactionContainerName);
            container.CreateIfNotExists();

            var accounts = new List<Transaction>();
            var blobs = container.ListBlobs();
            foreach (var blob in blobs)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob.Uri.Segments[2]);
                var document = blockBlob.DownloadText();
                transaction = JsonConvert.DeserializeObject<Transaction>(document);
                accounts.Add(transaction);
            }

            return accounts;
        }

        public static void SetTransaction(Transaction transaction)
        {
            CloudBlobContainer container = BlobClient.GetContainerReference(TransactionContainerName);
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(transaction.Id);
            var document = JsonConvert.SerializeObject(transaction, Formatting.Indented);
            blockBlob.UploadText(document);
        }

        public static IEnumerable<Account> GetAccounts()
        {
            Account account = null;

            CloudBlobContainer container = BlobClient.GetContainerReference(AccountContainerName);
            container.CreateIfNotExists();

            var accounts = new List<Account>();
            var blobs = container.ListBlobs();
            foreach (var blob in blobs)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob.Uri.Segments[2]);
                var document = blockBlob.DownloadText();
                account = JsonConvert.DeserializeObject<Account>(document);
                accounts.Add(account);
            }

            return accounts;
        }

        public static Account GetAccount(string accountId)
        {
            Account account = null;

            CloudBlobContainer container = BlobClient.GetContainerReference(AccountContainerName);
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(accountId);
            if (blockBlob.Exists())
            {
                var document = blockBlob.DownloadText();
                account = JsonConvert.DeserializeObject<Account>(document);
            }

            return account;
        }

        public static void SetAccount(Account account)
        {
            CloudBlobContainer container = BlobClient.GetContainerReference(AccountContainerName);
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(account.Id);
            var document = JsonConvert.SerializeObject(account, Formatting.Indented);
            blockBlob.UploadText(document);
        }
    }
}
