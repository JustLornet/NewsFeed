using System.Reflection;

namespace StreamBytesReader.Tests
{
    public class StreamParserTests
    {
        private Stream _sampleStream;
        private readonly string _sampleFullPath;

        private readonly string _sampleText = "first row\nsecond row\nsome text some text";

        public StreamParserTests()
        {
            // ����������� ���� �� ������� � �������
            var currentAsmPath = Assembly.GetExecutingAssembly().Location;
            var testFileDirectory = Path.GetDirectoryName(currentAsmPath);
            _sampleFullPath = Path.Combine(testFileDirectory!, "SampleText.txt");
        }

        /// <summary>
        /// ���������� ������ � ����� � �������� ����� ������ ������
        /// </summary>
        [SetUp]
        public void RefreshStream()
        {
            if (_sampleStream is not null)
                _sampleStream.Close();

            _sampleStream = File.OpenRead(_sampleFullPath);
        }

        [Test]
        public void DefaultStreamParameters()
        {
            // arrange
            var streamReader = new StreamParser(_sampleStream);
            
            // act
            var msg = streamReader.StreamMessage;

            // assert
            Assert.That(msg, Is.EqualTo(_sampleText));
        }

        [Test]
        public void NotDefaultBufferSize()
        {
            // arrange
            var streamReader = new StreamParser(_sampleStream, 8);

            // act
            var msg = streamReader.StreamMessage;

            // assert
            Assert.That(msg, Is.EqualTo(_sampleText));
        }

        [Test]
        public void DefaultStreamParametersNullStream()
        {
            // arrange
            Stream nullStream = null!;

            // act
            var exc = Assert.Throws<ArgumentNullException>(() => new StreamParser(nullStream));

            // assert
            Assert.That(exc, Is.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void DefaultStreamParametersNotReadingStream()
        {
            // act
            _sampleStream?.Dispose();
            var exc = Assert.Throws<ArgumentException>(() => new StreamParser(_sampleStream!));

            // assert
            Assert.That(exc.Message, Is.EqualTo("���������� ������ ������ ���������� ��������"));
        }

        [Test]
        public void DefaultStreamParametersReturnArray()
        {
            // arrange
            var streamReader = new StreamParser(_sampleStream);
            
            // act
            var msg = streamReader.SplittedStreamMessage;

            // assert
            Assert.That(msg, Is.EqualTo(_sampleText.Split("\n")));
        }

        [Test]
        public void ReleaseStreamAfterReading()
        {
            // arrange
            var streamReader = new StreamParser(_sampleStream);

            // act
            _ = streamReader.SplittedStreamMessage;
            _sampleStream?.Dispose();

            var msg = streamReader.SplittedStreamMessage;

            // assert
            Assert.That(msg, Is.EqualTo(_sampleText.Split("\n")));
        }

        [Test]
        public void ReleaseStreamBeforeReading()
        {
            // arrange
            var streamReader = new StreamParser(_sampleStream);

            // act
            _sampleStream?.Dispose();

            var exc = Assert.Throws<InvalidOperationException>(() => _ = streamReader.SplittedStreamMessage);

            // assert
            Assert.That(exc.Message, Is.EqualTo("���������� �������� ������ Stream"));
        }
    }
}