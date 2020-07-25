using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xunit;

using NChista.StoredProcedureCore.Tests;
using NChista.StoredProcedureCore.Extensions;
using NChista.StoredProcedureCore.Exceptions;

namespace NChista.StoredProcedureCore.ITests
{
    public class StoredProcedure_Tests : IClassFixture<SqlDatabaseFixture>
    {
        SqlDatabaseFixture _fixture;

        public StoredProcedure_Tests(SqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddParameters_PassNullModel_ShouldThrowException()
        {
            //Arrange
            SpParameters spParameters = null;
            
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                _fixture.Context.StoredProcedure("usp_no_result")
                        .AddParameters(spParameters)
            );
        }

        [Fact]
        public void StoredProcedure_PassNullSpName_ShouldThrowException()
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>
                _fixture.Context.StoredProcedure(null)
            );
        }

        [Fact]
        public void StoredProcedure_PassEmptySpName_ShouldThrowException()
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>
                _fixture.Context.StoredProcedure(string.Empty)
            );
        }

        [Fact]
        public async Task ExecuteReaderAsync_GetNoResult_ShouldReturnNull()
        {
            //Arrange
            List<TestStudent> actualResult = null;

            //Aact
            await _fixture.Context.StoredProcedure("usp_no_result")
                    .ExecuteReaderAsync(async reader => actualResult = await reader.GetResultAsync<TestStudent>());

            //Assert
            Assert.Null(actualResult);
        }

        [Fact]
        public async Task ExecuteReaderAsync_GetTwoResult_ShouldReturnResults()
        {
            //Arrange
            List<TestStudent> actualFirstResult = null;
            List<TestStudent> actualSecondResult = null;

            //Aact
            await _fixture.Context.StoredProcedure("usp_two_results")
                    .ExecuteReaderAsync(async reader =>
                    {
                        actualFirstResult = await reader.GetResultAsync<TestStudent>();
                        actualSecondResult = await reader.GetResultAsync<TestStudent>();
                    });

            //Assert
            Assert.NotNull(actualFirstResult);
            Assert.NotEmpty(actualFirstResult);
            Assert.NotNull(actualSecondResult);
            Assert.NotEmpty(actualSecondResult);

            for (int i = 0; i < _fixture.TestStudensModels.Count; i++)
            {
                Assert.True(actualFirstResult[i].Equals(_fixture.TestStudensModels[i]));
                Assert.True(actualSecondResult[i].Equals(_fixture.TestStudensModels[i]));
            }
        }

        [Fact]
        public async Task ExecuteReaderAsync_GetMoreResults_ShouldThrowException()
        {
            //Assert
            await Assert.ThrowsAsync<ResultOutOfRangeException>(async () =>
                await _fixture.Context.StoredProcedure("usp_two_results")
                        .ExecuteReaderAsync(async reader =>
                        {
                            await reader.GetResultAsync<TestStudent>();
                            await reader.GetResultAsync<TestStudent>();
                            await reader.GetResultAsync<TestStudent>();
                        })
            );

        }

        [Fact]
        public void ExecuteReader_GetNoResult_ShouldReturnNull()
        {
            //Arrange
            List<TestStudent> actualResult = null;

            //Aact
            _fixture.Context.StoredProcedure("usp_no_result")
                    .ExecuteReader(async reader => actualResult = await reader.GetResultAsync<TestStudent>());

            //Assert
            Assert.Null(actualResult);
        }

        [Fact]
        public void ExecuteReader_GetTwoResult_ShouldReturnResults()
        {
            //Arrange
            List<TestStudent> actualFirstResult = null;
            List<TestStudent> actualSecondResult = null;

            //Aact
            _fixture.Context.StoredProcedure("usp_two_results")
                    .ExecuteReader(async reader =>
                    {
                        actualFirstResult = await reader.GetResultAsync<TestStudent>();
                        actualSecondResult = await reader.GetResultAsync<TestStudent>();
                    });

            //Assert
            Assert.NotNull(actualFirstResult);
            Assert.NotEmpty(actualFirstResult);
            Assert.NotNull(actualSecondResult);
            Assert.NotEmpty(actualSecondResult);

            for (int i = 0; i < _fixture.TestStudensModels.Count; i++)
            {
                Assert.True(actualFirstResult[i].Equals(_fixture.TestStudensModels[i]));
                Assert.True(actualSecondResult[i].Equals(_fixture.TestStudensModels[i]));
            }
        }

        [Fact]
        public void ExecuteReader_GetMoreResults_ShouldThrowException()
        {
            //Assert
            Assert.Throws<ResultOutOfRangeException>(() =>
                _fixture.Context.StoredProcedure("usp_two_results")
                        .ExecuteReader(reader =>
                        {
                            reader.GetResult<TestStudent>();
                            reader.GetResult<TestStudent>();
                            reader.GetResult<TestStudent>();
                        })
            );

        }

        [Fact]
        public async Task ExecuteNonQueryAsync_InsertStudent_ShouldInsert()
        {
            //Arrange
            var studentNo = 9;
            var expectedStudent = new TestStudent() {
                Id = studentNo,
                FirstName = "FirstName" + studentNo,
                LastName = "LastName" + studentNo,
                Birthdate = new DateTime(2020, 01, 01),
                EmailAddress = "EmailAddress" + studentNo
            };

            var insertStudentParameters = new InsertStudentParameters()
            { 
                Id = expectedStudent.Id,
                FirstName = expectedStudent.FirstName,
                LastName = expectedStudent.LastName,
                Birthdate = expectedStudent.Birthdate,
                Email = expectedStudent.EmailAddress
            };

            //Aact
            await _fixture.Context.StoredProcedure("usp_insert")
                .AddParameters(insertStudentParameters)
                .ExecuteNonQueryAsync();

            var actualStudent = _fixture.Context.TestStudents.Find(studentNo);

            //Assert
            Assert.NotNull(actualStudent);
            Assert.Equal(expectedStudent, actualStudent);
        }

        [Fact]
        public void ExecuteNonQuery_InsertStudent_ShouldInsert()
        {
            //Arrange
            var studentNo = 8;
            var expectedStudent = new TestStudent()
            {
                Id = studentNo,
                FirstName = "FirstName" + studentNo,
                LastName = "LastName" + studentNo,
                Birthdate = new DateTime(2020, 01, 01),
                EmailAddress = "EmailAddress" + studentNo
            };

            var insertStudentParameters = new InsertStudentParameters()
            {
                Id = expectedStudent.Id,
                FirstName = expectedStudent.FirstName,
                LastName = expectedStudent.LastName,
                Birthdate = expectedStudent.Birthdate,
                Email = expectedStudent.EmailAddress
            };

            //Aact
            _fixture.Context.StoredProcedure("usp_insert")
                .AddParameters(insertStudentParameters)
                .ExecuteNonQuery();

            var actualStudent = _fixture.Context.TestStudents.Find(studentNo);

            //Assert
            Assert.NotNull(actualStudent);
            Assert.Equal(expectedStudent, actualStudent);
        }
    }
}
