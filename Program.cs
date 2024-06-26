using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Signatures;

namespace HelloPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // using var document = new Document(new PdfDocument(new PdfWriter("helloworld-pdf.pdf")));
            // document.Add(new Paragraph("Isaque!"));

            verifySignatures("alexandre-manda-redes-sociais.pdf");
        }


        public static void verifySignatures(string path)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            IList<string> names = signUtil.GetSignatureNames();



            Console.WriteLine(path);
            foreach (var name in names)
            {
                Console.WriteLine("===== " + name + " =====");
                verifySignature(signUtil, name);
            }

            pdfDoc.Close();
        }

        public static PdfPKCS7 verifySignature(SignatureUtil signUtil, String name)
        {
            PdfPKCS7 pkcs7 = signUtil.ReadSignatureData(name);

            pkcs7.GetCertificates()
            .ToList()
            .ForEach(cert => Console.WriteLine("IssuerDN: " + cert.GetIssuerDN() + "\r\nSubjectDN: " + cert.GetSubjectDN()));

            Console.WriteLine("A assinatura cobre todo o documento: " + signUtil.SignatureCoversWholeDocument(name));
            Console.WriteLine("Revisão de documento: " + signUtil.GetRevision(name) + " of " + signUtil.GetTotalRevisions());
            Console.WriteLine("Verificação de integridade OK? " + pkcs7.VerifySignatureIntegrityAndAuthenticity());

            return pkcs7;
        }
    }
}