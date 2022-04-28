using ProradisEx1.Data.Dto;
using ProradisEx1.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProradisEx1.Service
{
    public class ProcessDataService : IProcessDataService<CityPayloadDto, ResponseDto>
    {
        private readonly ISendRequestService<CityPayloadDto, ResponseDto> _sendRequestService;
        public ProcessDataService(ISendRequestService<CityPayloadDto, ResponseDto> sendReuestService)
        {
            _sendRequestService = sendReuestService;
        }
        public async Task<ResponseDto> Process()
        {
            try
            {
                string path = $@"{Directory.GetCurrentDirectory()}\input.csv";
                using (var reader = new StreamReader(path))
                {
                    var CityDetailList = new List<CsvDto>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        var item = new CsvDto
                        {
                            Nome = values[0],
                            Cidade = values[1],
                            Idade = int.Parse(values[2])
                        };

                        CityDetailList.Add(item);
                    }

                    return await _sendRequestService
                        .Send(GetCityPayload(CityDetailList));
                }
            }
            catch
            {
                throw;
            }
        }

        private CityPayloadDto GetCityPayload(IList<CsvDto> param)
        {
            var _medias = param
                .GroupBy(x => RemoveDiacritics(x.Cidade.ToUpper()))
                .Select(item => new Media 
                { 
                    cidade = item.Key,
                    idade = Math.Round((double)(item.Sum(x => x.Idade))
                        /(item.Select(x => x.Nome)
                        .ToList()
                        .Count()), 2)
                }).ToList();

            var result = new CityPayloadDto
            {
                medias = _medias
            };

            return result;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
