// certgen.cs
#r "nuget: BouncyCastle, 1.8.9"

using System;
using System.Text;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Operators;

var root = GenerateCertificate("CN=RootCA", null, null, true);
var intermediate = GenerateCertificate("CN=IntermediateCA", root.Item1, root.Item2, true);
var leaf = GenerateCertificate("CN=LeafCert", intermediate.Item1, intermediate.Item2, false);

Console.WriteLine("Root CA (Base64):\n" + ToBase64(root));
Console.WriteLine("Intermediate CA (Base64):\n" + ToBase64(intermediate));
Console.WriteLine("Leaf Cert (Base64):\n" + ToBase64(leaf));

static Tuple<X509Certificate, AsymmetricKeyParameter> GenerateCertificate(
    string subjectName,
    X509Certificate issuerCert,
    AsymmetricKeyParameter issuerKey,
    bool isCA)
{
    var random = new SecureRandom(new CryptoApiRandomGenerator());
    var keyGen = new RsaKeyPairGenerator();
    keyGen.Init(new KeyGenerationParameters(random, 2048));
    var keyPair = keyGen.GenerateKeyPair();

    var certGen = new X509V3CertificateGenerator();
    var subjectDN = new X509Name(subjectName);
    var issuerDN = issuerCert != null ? issuerCert.SubjectDN : subjectDN;

    certGen.SetSerialNumber(BigInteger.ProbablePrime(120, random));
    certGen.SetIssuerDN(issuerDN);
    certGen.SetSubjectDN(subjectDN);
    certGen.SetNotBefore(DateTime.UtcNow.Date);
    certGen.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));
    certGen.SetPublicKey(keyPair.Public);
    certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(isCA));

    var signer = new Asn1SignatureFactory("SHA256WithRSA", issuerKey ?? keyPair.Private, random);
    var cert = certGen.Generate(signer);

    return Tuple.Create(cert, keyPair.Private);
}

static string ToBase64(Tuple<X509Certificate, AsymmetricKeyParameter> certAndKey)
{
    var sb = new StringBuilder();
    using (var sw = new StringWriter(sb))
    {
        var pemWriter = new PemWriter(sw);
        pemWriter.WriteObject(certAndKey.Item1);
        pemWriter.WriteObject(certAndKey.Item2);
    }
    var bytes = Encoding.UTF8.GetBytes(sb.ToString());
    return Convert.ToBase64String(bytes);
}
