namespace UnitOfWork
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseCrudController<T, M> : ControllerBase where T : class
    {
        public readonly IAdapter<T, M> _adapter;

        public readonly ICrudService<T,M> _crudService;

        public BaseCrudController(IAdapter<T, M> adapter, ICrudService<T, M> crudService)
        {
            _adapter = adapter;
            _crudService = crudService;
        }

        [HttpGet]
        [Route("Get")]
        public virtual async Task<IActionResult> GetAsync(int id)
        {
            var entity = await _crudService.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                Success = true,
                Data = _adapter.Adapt(entity)
            });
        }

        [Authorize]
        [Route("update")]
        [HttpPut]
        public virtual async Task<IActionResult> PutAsync(M model)
        {
            try
            {
                _crudService.ValidateAndThrow(model);
                var entity = _adapter.Adapt(model);
                await _crudService.UpdateAsync(entity);
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
                _crudService.ValidateAndThrow(model);
                var entity = _adapter.Adapt(model);

                var inserted = await _crudService.InsertAsync(entity);
                
                return Ok(new
                {
                    Success = true,
                    Data = _adapter.Adapt(inserted)
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
        [HttpDelete]
        [Route("delete")]
        public virtual async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await _crudService.DeleteAsync(id);
            return Ok(new
            {
                Success = true
            });
        }
    }
}