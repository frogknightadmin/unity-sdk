﻿using Chaos.NaCl;
using Google.Protobuf;
using System;
using System.Threading.Tasks;

namespace Loom.Unity3d
{
    /// <summary>
    /// Signs transactions.
    /// </summary>
    public class SignedTxMiddleware : ITxMiddlewareHandler
    {
        /// <summary>
        /// The private key that should be used to sign txs.
        /// </summary>
        public byte[] PrivateKey { get; set; }

        /// <summary>
        /// Creates middlware that signs txs with the given key.
        /// </summary>
        /// <param name="privateKey">The private key that should be used to sign txs.</param>
        public SignedTxMiddleware(byte[] privateKey)
        {
            this.PrivateKey = privateKey;
        }

        public Task<byte[]> Handle(byte[] txData)
        {
            var sig = CryptoUtils.Sign(txData, this.PrivateKey);

            var signer = new Signer
            {
                Signature = ByteString.CopyFrom(sig.Signature),
                PublicKey = ByteString.CopyFrom(sig.PublicKey)
            };

            var signedTx = new SignedTx
            {
                Inner = ByteString.CopyFrom(txData),
                Signers = { signer }
            };

            return Task.FromResult(signedTx.ToByteArray());
        }
    }
}
