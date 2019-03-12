using com.google.typography.font.sfntly;
using com.google.typography.font.sfntly.data;
using com.google.typography.font.sfntly.table.core;
using com.google.typography.font.tools.conversion.eot;
using com.google.typography.font.tools.conversion.woff;
using com.google.typography.font.tools.sfnttool;
using com.google.typography.font.tools.subsetter;
using CommandLine;
using java.io;
using java.lang;
using java.util;
using Console = System.Console;
using Exception = System.Exception;

namespace GFontClipper
{
    class Program
    {
        static bool strip = false;
        static bool woff = false;
        static bool eot = false;
        static bool mtx = false;
        
        static void Main(string[] args)
        {
            //args = new[] { "-f", "F:/GFontClipper/str.txt", "-s", "F:/GFontClipper/RZRXJXH.ttf", "-t", "F:/GFontClipper/test.ttf" };
            Parser.Default.ParseArguments<Options>(args).WithParsed(ClicpFont).WithNotParsed(errs =>
            {
                foreach (var error in errs)
                {
                    Console.WriteLine(error);
                }
            });
        }

        static void ClicpFont(Options option)
        {
            try
            {
                string subString = System.IO.File.ReadAllText(option.subFile);

                var oriFile = new File(option.srcFont);
                var newFile = new File(option.tarFont);
                FontFactory fontFactory = FontFactory.getInstance();
                FileInputStream fileStream = new FileInputStream(oriFile);
                byte[] bytes = new byte[(int)oriFile.length()];
                fileStream.read(bytes);
                Font[] fonts = null;
                fonts = fontFactory.loadFonts(bytes);
                Font font1 = fonts[0];
                ArrayList arrayList = new ArrayList();
                arrayList.add(CMapTable.CMapId.WINDOWS_BMP);
                Object object2 = null;

                Font font2 = font1;
                Object object3;
                if (subString.Length > 0)
                {
                    object2 = new RenumberingSubsetter(font2, fontFactory);
                    ((Subsetter)object2).setCMaps(arrayList, 1);
                    object3 = (Object)GlyphCoverage.getGlyphCoverage(font1, subString);
                    ((Subsetter)object2).setGlyphs((List)object3);
                    var hashSet = new HashSet();
                    hashSet.add(Integer.valueOf(Tag.GDEF));
                    hashSet.add(Integer.valueOf(Tag.GPOS));
                    hashSet.add(Integer.valueOf(Tag.GSUB));
                    hashSet.add(Integer.valueOf(Tag.kern));
                    hashSet.add(Integer.valueOf(Tag.hdmx));
                    hashSet.add(Integer.valueOf(Tag.vmtx));
                    hashSet.add(Integer.valueOf(Tag.VDMX));
                    hashSet.add(Integer.valueOf(Tag.LTSH));
                    hashSet.add(Integer.valueOf(Tag.DSIG));
                    hashSet.add(Integer.valueOf(Tag.intValue(new byte[] { 109, 111, 114, 116 })));
                    hashSet.add(Integer.valueOf(Tag.intValue(new byte[] { 109, 111, 114, 120 })));
                    ((Subsetter)object2).setRemoveTables(hashSet);
                    font2 = ((Subsetter)object2).subset().build();
                }
                if (strip)
                {
                    object2 = new HintStripper(font2, fontFactory);
                    object3 = new HashSet();
                    ((Set)object3).add(Integer.valueOf(Tag.fpgm));
                    ((Set)object3).add(Integer.valueOf(Tag.prep));
                    ((Set)object3).add(Integer.valueOf(Tag.cvt));
                    ((Set)object3).add(Integer.valueOf(Tag.hdmx));
                    ((Set)object3).add(Integer.valueOf(Tag.VDMX));
                    ((Set)object3).add(Integer.valueOf(Tag.LTSH));
                    ((Set)object3).add(Integer.valueOf(Tag.DSIG));
                    ((Subsetter)object2).setRemoveTables((Set)object3);
                    font2 = ((Subsetter)object2).subset().build();
                }
                object2 = new FileOutputStream(newFile);
                if (woff)
                {
                    object3 = new WoffWriter().convert(font2);
                    ((WritableFontData)object3).copyTo((OutputStream)object2);
                }
                else if (eot)
                {
                    object3 = new EOTWriter(mtx).convert(font2);
                    ((WritableFontData)object3).copyTo((OutputStream)object2);
                }
                else
                {
                    fontFactory.serializeFont(font2, (OutputStream)object2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    class Options
    {
        [Option('f', "file", Required = true, HelpText = "转换字体文件")]
        public string subFile { get; set; }

        [Option('s', "src", Required = true, HelpText = "源字体地址")]
        public string srcFont { get; set; }

        [Option('t', "target", Required = true, HelpText = "转换成的地址文件")]
        public string tarFont { get; set; }
    }
}
