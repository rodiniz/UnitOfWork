namespace UnitOfWork
{
    using System;
    using System.Threading.Tasks;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseCrudController<T, M> : ControllerBase where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<T> _repository;

        public readonly IAdapter<T, M> _adapter;

        public readonly IValidator<M> _validator;


        public BaseCrudController(IUnitOfWork unitOfWork, IAdapter<T, M> adapter, IValidator<M> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T>();
            _adapter = adapter;
            _validator = validator;
        }


        [HttpGet]
        [Route("Get")]
        public virtual async Task<IActionResult> GetAsync(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                Success = true,
                Data = _adapter.convertToModel(entity)
            });
        }

        [Authorize]
        [Route("update")]
        [HttpPut]
        public virtual async Task<IActionResult> PutAsync(M model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
                var entity = _adapter.convertFromModel(model);
                _repository.Update(entity);

                await _unitOfWork.SaveChangesAsync();
                return Ok(new
                {
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    ex.Message
                });
            }

        }

        [Authorize]
        [Route("create")]
        [HttpPost]
        public virtual async Task<IActionResult> PostAsync([FromBody] M model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
                var entity = _adapter.convertFromModel(model);

                var inserted = await _repository.InsertAsync(entity);

                await _unitOfWork.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Data = inserted.Entity
                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    Success = false,
                    ex.Message
                });
            }


        }

        /// <summary>
        /// Apaga a entidade do banco de dados.
        /// </summary>
        /// <param name="entidade">A entidade que será apagada</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("delete")]
        public virtual async Task<IActionResult> DeleteAsync(T entidade)
        {
            _repository.Delete(entidade);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new
            {
                Success = true
            });
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteById")]
        public virtual async Task<IActionResult> DeleteByIdAsync(int id)
        {
            _repository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new
            {
                Success = true
            });
        }
    }
}
