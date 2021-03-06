﻿//
// PublicKeyAlgorithm.cs
//
// Author(s): David Lechner <david@lechnology.com>
//
// Copyright (c) 2012,2015 David Lechner
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Diagnostics;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using dlech.SshAgentLib.Crypto;

namespace dlech.SshAgentLib
{
  /// <summary>
  /// Public Key Algorithms supports by SSH
  /// </summary>
  public enum PublicKeyAlgorithm
  {
    SSH_RSA,
    SSH_DSS,
    ECDSA_SHA2_NISTP256,
    ECDSA_SHA2_NISTP384,
    ECDSA_SHA2_NISTP521,
    ED25519,
  }

  public static class PublicKeyAlgorithmExt
  {
    /* defined by RFC 4253 - http://www.ietf.org/rfc/rfc4253.txt */
    public const string ALGORITHM_DSA_KEY = "ssh-dss";
    public const string ALGORITHM_RSA_KEY = "ssh-rsa";
    public const string ALGORITHM_PGP_RSA_SIGN_CERT = "pgp-sign-rsa";
    public const string ALGORITHM_PGP_DSA_SIGN_CERT = "pgp-sign-dss";

    /* defined by OpenSSH PROTOCOL.agent - http://api.libssh.org/rfc/PROTOCOL.agent */
    public const string OPENSSH_CERT_V00_SUFFIX = "-cert-v00@openssh.com";
    public const string OPENSSH_CERT_V01_SUFFIX = "-cert-v01@openssh.com";
    //public const string ALGORITHM_DSA_KEY = "ssh-dss";
    public const string ALGORITHM_DSA_CERT = 
      ALGORITHM_DSA_KEY + OPENSSH_CERT_V00_SUFFIX;
    //public const string ALGORITHM_RSA_KEY = "ssh-rsa";
    public const string ALGORITHM_RSA_CERT = ALGORITHM_DSA_KEY + OPENSSH_CERT_V00_SUFFIX;
    public const string ALGORITHM_ECDSA_SHA2_PREFIX = "ecdsa-sha2-";
    public const string EC_ALGORITHM_NISTP256 = "nistp256";
    public const string EC_ALGORITHM_NISTP384 = "nistp384";
    public const string EC_ALGORITHM_NISTP521 = "nistp521";
    public const string ALGORITHM_ECDSA_SHA2_NISTP256_KEY =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP256;
    public const string ALGORITHM_ECDSA_SHA2_NISTP384_KEY =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP384;
    public const string ALGORITHM_ECDSA_SHA2_NISTP521_KEY =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP521;
    public const string ALGORITHM_ECDSA_SHA2_NISTP256_CERT =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP256 + OPENSSH_CERT_V01_SUFFIX;
    public const string ALGORITHM_ECDSA_SHA2_NISTP384_CERT =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP384 + OPENSSH_CERT_V01_SUFFIX;
    public const string ALGORITHM_ECDSA_SHA2_NISTP521_CERT =
      ALGORITHM_ECDSA_SHA2_PREFIX + EC_ALGORITHM_NISTP521 + OPENSSH_CERT_V01_SUFFIX;

    // not in PROTOCOL.agent...yet
    public const string ALGORITHM_ED25519 = "ssh-ed25519";
    public const string ALGORITHM_ED25519_CERT = ALGORITHM_ED25519 + OPENSSH_CERT_V01_SUFFIX;

    public static string GetIdentifierString(this PublicKeyAlgorithm aPublicKeyAlgorithm)
    {
      switch (aPublicKeyAlgorithm) {
        case PublicKeyAlgorithm.SSH_RSA:
          return ALGORITHM_RSA_KEY;
        case PublicKeyAlgorithm.SSH_DSS:
          return ALGORITHM_DSA_KEY;
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP256:
          return ALGORITHM_ECDSA_SHA2_NISTP256_KEY;
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP384:
          return ALGORITHM_ECDSA_SHA2_NISTP384_KEY;
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP521:
          return ALGORITHM_ECDSA_SHA2_NISTP521_KEY;
        case PublicKeyAlgorithm.ED25519:
          return ALGORITHM_ED25519;
        default:
          Debug.Fail("Unknown algorithm");
          throw new Exception("Unknown algorithm");
      }
    }

    public static ISigner GetSigner(this PublicKeyAlgorithm aPublicKeyAlgorithm)
    {
      switch (aPublicKeyAlgorithm) {
        case PublicKeyAlgorithm.SSH_RSA:
          return SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id);
        case PublicKeyAlgorithm.SSH_DSS:
          return SignerUtilities.GetSigner(X9ObjectIdentifiers.IdDsaWithSha1.Id);
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP256:
          return SignerUtilities.GetSigner(X9ObjectIdentifiers.ECDsaWithSha256.Id);
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP384:
          return SignerUtilities.GetSigner(X9ObjectIdentifiers.ECDsaWithSha384.Id);
        case PublicKeyAlgorithm.ECDSA_SHA2_NISTP521:
          return SignerUtilities.GetSigner(X9ObjectIdentifiers.ECDsaWithSha512.Id);
        case PublicKeyAlgorithm.ED25519:
          return new Ed25519Signer();
        default:
          Debug.Fail("Unknown algorithm");
          throw new Exception("Unknown algorithm");
      }
    }
  }
}
