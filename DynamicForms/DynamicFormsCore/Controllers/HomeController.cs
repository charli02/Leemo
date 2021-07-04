using DynamicFormsCore.Common;
using DynamicFormsCore.Models;
using DynamicFormsCore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicFormsCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbformsContext _db;
        public HomeController(ILogger<HomeController> logger
            , DbformsContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var modal = GetModel();
            return View(modal);
            //return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {

            //var myForm = form;
            var formSetup = GetModel();

            //var formSetup = GetFormData();

            //foreach (var formitem in form)
            //{
            //    var submittedformItem = formitem;                               

            //    //if (formSetup.Control.ItemList.Any(w => w.Key == submittedformItem.Key))
            //    //{
            //    //    FormItem formItemTemp = formSetup.FormItems.Single(w => w.Key == submittedformItem.Key).Value;
            //    //    formItemTemp.Value = submittedformItem.Value;
            //    //}
            //}

            var formSubmission = new DataFormSubmission();
            formSubmission.SubmitDateUTC = DateTime.UtcNow;
            formSubmission.DataFormId = _db.DataForm.FirstOrDefault().Id;
            _db.DataFormSubmission.Add(formSubmission);
            _db.SaveChanges();



            foreach (var item in formSetup.DataFormSection)
            {
                foreach (var field in item.DataFormField)
                {
                    #region Render Control

                    //Image
                    if (field.DataFormFieldType.FieldTypeName == FormInputType.Image.ToString())
                    {
                        //field.InputType = FormInputType.Image;
                    }

                    //Date
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Date.ToString())
                    {
                        //field.InputType = FormInputType.Date;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldDateValue = DateTime.ParseExact(itemValue,"yyyy-MM-dd", CultureInfo.InvariantCulture);

                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //Text
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Text.ToString())
                    {
                        //field.InputType = FormInputType.Text;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //CheckBox
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.CheckBox.ToString())
                    {
                        //field.InputType = FormInputType.CheckBox;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldBoolValue = (itemValue != null ? true : false );
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //CheckBoxList
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.CheckBoxList.ToString())
                    {
                        //////field.InputType = FormInputType.CheckBoxList;
                        //string itemValue = "";

                        //foreach (var fieldItem in field.ControlXML.ItemList.Items)
                        //{
                        //    var controlName = item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name;
                        //    itemValue += form[controlName].ToString() + "|";
                        //    //renderControl += "<input type='checkbox' id='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "' "
                        //    //            + " name='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "' "
                        //    //            + " value='" + fieldItem.Value + "' "
                        //    //            + (!string.IsNullOrEmpty(fieldItem.Selected) ? "checked" : "")
                        //    //            + " >"
                        //    //            + "<label for='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "'>" + fieldItem.Text + "</label><br>";
                        //}

                        //var formSubmissionItem = new DataFormSubmissionItem();
                        //formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        //formSubmissionItem.DataFormFieldId = field.Id;
                        //formSubmissionItem.FieldStringValue = itemValue;
                        //_db.Add(formSubmissionItem);
                        //_db.SaveChanges();
                    }

                    //Number
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Number.ToString())
                    {
                        //field.InputType = FormInputType.Number;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldIntValue = Convert.ToInt32(itemValue);
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    ////File
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.File.ToString())
                    //{
                    //    //field.InputType = FormInputType.File;
                    //}

                    //Radio
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Radio.ToString())
                    {
                        //field.InputType = FormInputType.Radio;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //Password
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Password.ToString())
                    {
                        //field.InputType = FormInputType.Password;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    ////Time
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Time.ToString())
                    //{
                    //    //field.InputType = FormInputType.Time;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    //var formSubmissionItem = new DataFormSubmissionItem();
                    //    //formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    //formSubmissionItem.DataFormFieldId = field.Id;
                    //    //formSubmissionItem. = itemValue;
                    //    //_db.Add(formSubmissionItem);
                    //    //_db.SaveChanges();

                    //}

                    ////URL
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.URL.ToString())
                    //{
                    //    //field.InputType = FormInputType.URL;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    //Color
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Color.ToString())
                    {
                        //field.InputType = FormInputType.Color;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    ////Hidden
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Hidden.ToString())
                    //{
                    //    //field.InputType = FormInputType.Hidden;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    //Email
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Email.ToString())
                    {
                        //field.InputType = FormInputType.Email;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //Range
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Range.ToString())
                    {
                        //field.InputType = FormInputType.Range;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldFloatValue = Convert.ToDouble(itemValue);
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();
                    }

                    //DateTimeLocal
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.DateTimeLocal.ToString())
                    {
                        //field.InputType = FormInputType.DateTimeLocal;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;

                        var dateValue = itemValue.Split('T')[0];
                        var timeValue = itemValue.Split('T')[1];

                        formSubmissionItem.FieldDateValue = DateTime.ParseExact(dateValue + " " + timeValue, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();

                    }

                    ////Month
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Month.ToString())
                    //{
                    //    //field.InputType = FormInputType.Month;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    ////Tel
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Tel.ToString())
                    //{
                    //    //field.InputType = FormInputType.Tel;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    ////Week
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Week.ToString())
                    //{
                    //    //field.InputType = FormInputType.Week;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.SelectList.ToString())
                    {
                        //field.InputType = FormInputType.SelectList;
                        var controlName = item.SectionName + "-" + field.FieldName;
                        var itemValue = form[controlName].ToString();

                        var formSubmissionItem = new DataFormSubmissionItem();
                        formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                        formSubmissionItem.DataFormFieldId = field.Id;
                        formSubmissionItem.FieldStringValue = itemValue;
                        _db.Add(formSubmissionItem);
                        _db.SaveChanges();

                    }
                    #endregion
                }
            }


            return View(formSetup);
        }

        //Guid formID
        public IActionResult GetForm()
        {
            var modal = GetModel();

            var formResult = _db.DataFormSubmission.Include(i => i.DataFormSubmissionItem).ThenInclude(i => i.DataFormField).FirstOrDefault();

            foreach(var item in formResult.DataFormSubmissionItem.ToList())
            {
                //foreach (var field in item.DataFormField)
                {
                    #region Render Control

                    //Image
                    if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Image.ToString())
                    {
                        //field.InputType = FormInputType.Image;
                    }

                    //Date
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Date.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldDateValue.Value.ToString("yyyy-MM-dd");
                    }

                    //Text
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Text.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;
                    }

                    //CheckBox
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.CheckBox.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = (item.FieldBoolValue.Value ? "On" : "");
                    }

                    ////CheckBoxList
                    //else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.CheckBoxList.ToString())
                    //{
                    //    modal.DataFormSection.FirstOrDefault()
                    //        .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                    //        .FieldValue = item.FieldTextValue;
                    //    //////field.InputType = FormInputType.CheckBoxList;
                    //    //string itemValue = "";

                    //    //foreach (var fieldItem in field.ControlXML.ItemList.Items)
                    //    //{
                    //    //    var controlName = item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name;
                    //    //    itemValue += form[controlName].ToString() + "|";
                    //    //    //renderControl += "<input type='checkbox' id='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "' "
                    //    //    //            + " name='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "' "
                    //    //    //            + " value='" + fieldItem.Value + "' "
                    //    //    //            + (!string.IsNullOrEmpty(fieldItem.Selected) ? "checked" : "")
                    //    //    //            + " >"
                    //    //    //            + "<label for='" + item.SectionName + "-" + field.FieldName + "-" + fieldItem.Name + "'>" + fieldItem.Text + "</label><br>";
                    //    //}

                    //    //var formSubmissionItem = new DataFormSubmissionItem();
                    //    //formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    //formSubmissionItem.DataFormFieldId = field.Id;
                    //    //formSubmissionItem.FieldStringValue = itemValue;
                    //    //_db.Add(formSubmissionItem);
                    //    //_db.SaveChanges();
                    //}

                    //Number
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Number.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldIntValue.Value.ToString();
                    }

                    ////File
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.File.ToString())
                    //{
                    //    //field.InputType = FormInputType.File;
                    //}

                    //Radio
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Radio.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;
                    }

                    //Password
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Password.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;
                    }

                    ////Time
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Time.ToString())
                    //{
                    //    //field.InputType = FormInputType.Time;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    //var formSubmissionItem = new DataFormSubmissionItem();
                    //    //formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    //formSubmissionItem.DataFormFieldId = field.Id;
                    //    //formSubmissionItem. = itemValue;
                    //    //_db.Add(formSubmissionItem);
                    //    //_db.SaveChanges();

                    //}

                    ////URL
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.URL.ToString())
                    //{
                    //    //field.InputType = FormInputType.URL;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    //Color
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Color.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;
                    }

                    ////Hidden
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Hidden.ToString())
                    //{
                    //    //field.InputType = FormInputType.Hidden;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    //Email
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Email.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;
                    }

                    //Range
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.Range.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldFloatValue.Value.ToString();
                    }

                    //DateTimeLocal
                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.DateTimeLocal.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldDateValue.Value.ToString("yyyy-MM-dd'T'HH:mm");

                    }

                    ////Month
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Month.ToString())
                    //{
                    //    //field.InputType = FormInputType.Month;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    ////Tel
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Tel.ToString())
                    //{
                    //    //field.InputType = FormInputType.Tel;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    ////Week
                    //else if (field.DataFormFieldType.FieldTypeName == FormInputType.Week.ToString())
                    //{
                    //    //field.InputType = FormInputType.Week;
                    //    var controlName = item.SectionName + "-" + field.FieldName;
                    //    var itemValue = form[controlName].ToString();

                    //    var formSubmissionItem = new DataFormSubmissionItem();
                    //    formSubmissionItem.DataFormSubmissionId = formSubmission.Id;
                    //    formSubmissionItem.DataFormFieldId = field.Id;
                    //    formSubmissionItem.FieldStringValue = itemValue;
                    //    _db.Add(formSubmissionItem);
                    //    _db.SaveChanges();
                    //}

                    else if (item.DataFormField.DataFormFieldType.FieldTypeName == FormInputType.SelectList.ToString())
                    {
                        modal.DataFormSection.FirstOrDefault()
                            .DataFormField.Where(d => d.Id == item.DataFormFieldId).FirstOrDefault()
                            .FieldValue = item.FieldStringValue;


                    }
                    #endregion
                }
            }


            return View("~/Views/Home/Index.cshtml",modal);
        }

        private DataFormViewModal GetModel()
        {
            //var _db = new DbformsContext();

            //var formsList = _db.DataFormSection.ToList();

            //var strXML = @"
            //<control attr1='test control'>
            //    <itemList group = 'test item group'>
            //          <item name = 'item 1' selected = '1' >
            //                <text>my text 1</text>       
            //                <value>my value 1</value>
            //          </item>
            //          <item name = 'item 2' selected = '0' >
            //                <text>my text2</text>       
            //                <value>my value2</value>
            //          </item>
            //          <item name = 'item 3' selected = '0' >
            //                <text>my text 3</text>       
            //                <value>my value 3</value>
            //          </item>
            //    </itemList>
            //</control>";

            //var serializeControl = new SerializeDeserialize<Control>();
            //Control deserialiedControl = serializeControl.DeserializeData(strXML);
            //deserialiedControl.Attr1 = "Week";
            ////Console.WriteLine("Employee ID : {0} ,  Name :{1} ,Profession : {2}", deserialiedEmployee.ID, deserialiedEmployee.Name, deserialiedEmployee.Profession);


            //return View(deserialiedControl);

            var dataForm = _db.DataForm
                            .Include(i => i.DataFormSection)
                            .ThenInclude(i => i.DataFormField)
                            .ThenInclude(i => i.DataFormFieldType)
                            .FirstOrDefault();

            //DataFormViewModal modal = new DataFormViewModal();
            //modal = (dataForm as DataFormViewModal);

            //modal.DataFormSection = modal.DataFormSection.ToList();
            //modal.DataFormSection.LastOrDefault().DataFormField
            //    = modal.DataFormSection.FirstOrDefault().DataFormField.ToList();

            var modal = ConvertObject<DataFormViewModal, DataForm>(dataForm);

            foreach(var item in modal.DataFormSection)
            {
                foreach (var field in item.DataFormField)
                {
                    #region Render Control

                    //Image
                    if (field.DataFormFieldType.FieldTypeName == FormInputType.Image.ToString())
                    {
                        field.InputType = FormInputType.Image;
                    }

                    //Date
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Date.ToString())
                    {
                        field.InputType = FormInputType.Date;
                    }

                    //Text
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Text.ToString())
                    {
                        field.InputType = FormInputType.Text;
                    }

                    //CheckBox
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.CheckBox.ToString())
                    {
                        field.InputType = FormInputType.CheckBox;
                    }

                    //CheckBoxList
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.CheckBoxList.ToString())
                    {
                        field.InputType = FormInputType.CheckBoxList;
                    }

                    //Number
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Number.ToString())
                    {
                        field.InputType = FormInputType.Number;
                    }

                    //File
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.File.ToString())
                    {
                        field.InputType = FormInputType.File;
                    }

                    //Radio
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Radio.ToString())
                    {
                        field.InputType = FormInputType.Radio;
                    }

                    //Password
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Password.ToString())
                    {
                        field.InputType = FormInputType.Password;
                    }

                    //Time
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Time.ToString())
                    {
                        field.InputType = FormInputType.Time;
                    }

                    //URL
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.URL.ToString())
                    {
                        field.InputType = FormInputType.URL;
                    }

                    //Color
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Color.ToString())
                    {
                        field.InputType = FormInputType.Color;
                    }

                    //Hidden
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Hidden.ToString())
                    {
                        field.InputType = FormInputType.Hidden;
                    }

                    //Email
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Email.ToString())
                    {
                        field.InputType = FormInputType.Email;
                    }

                    //Range
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Range.ToString())
                    {
                        field.InputType = FormInputType.Range;
                    }

                    //DateTimeLocal
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.DateTimeLocal.ToString())
                    {
                        field.InputType = FormInputType.DateTimeLocal;
                    }

                    //Month
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Month.ToString())
                    {
                        field.InputType = FormInputType.Month;
                    }

                    //Tel
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Tel.ToString())
                    {
                        field.InputType = FormInputType.Tel;
                    }

                    //Week
                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.Week.ToString())
                    {
                        field.InputType = FormInputType.Week;
                    }

                    else if (field.DataFormFieldType.FieldTypeName == FormInputType.SelectList.ToString())
                    {
                        field.InputType = FormInputType.SelectList;
                    }
                    #endregion
                }
            }

            var serializeControl = new SerializeDeserialize<Control>();
            for (int i = 0; i < modal.DataFormSection.FirstOrDefault().DataFormField.Count; i++)
            {
                var item = modal.DataFormSection.FirstOrDefault().DataFormField[i];
                if (item.DataFormFieldType.FieldTypeName == FormInputType.CheckBoxList.ToString()
                    || item.DataFormFieldType.FieldTypeName == FormInputType.Radio.ToString()
                    || item.DataFormFieldType.FieldTypeName == FormInputType.SelectList.ToString())
                {
                    if (!string.IsNullOrEmpty(item.FieldXML))
                    {
                        item.ControlXML = serializeControl.DeserializeData(item.FieldXML);
                    }
                }
            }


            //modal.Control = deserialiedControl;

            return modal;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Object Convert
                
        protected T ConvertObject<T, X>(X result)
        {
            var derivedClassInstance = Activator.CreateInstance<T>();
            var derivedType = derivedClassInstance.GetType();

            var properties = result.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propToSet = derivedType.GetProperty(property.Name);
                if (propToSet.SetMethod != null)
                {
                    propToSet.SetValue(derivedClassInstance, property.GetValue(result));
                }
            }
            return derivedClassInstance;
        }

        protected List<T> ConvertObject<T, X>(List<X> listResult)
        {
            var derivedList = new List<T>();
            foreach (var r in listResult)
            {
                //can cope with this - since there will not ever be many iterations
                derivedList.Add(ConvertObject<T, X>(r));
            }
            return derivedList;
        }

        #endregion
    }

    //public static T ConvertToDerived<T>(object baseObj) where T : new()
    //{
    //    var derivedObj = new T();
    //    var members = baseObj.GetType().GetMembers();
    //    foreach (var member in members)
    //    {
    //        object val = null;
    //        if (member.MemberType == MemberTypes.Field)
    //        {
    //            val = ((FieldInfo)member).GetValue(baseObj);
    //            ((FieldInfo)member).SetValue(derivedObj, val);
    //        }
    //        else if (member.MemberType == MemberTypes.Property)
    //        {
    //            val = ((PropertyInfo)member).GetValue(baseObj);
    //            if (val is IList && val.GetType().IsGenericType)
    //            {
    //                var listType = val.GetType().GetGenericArguments().Single();
    //                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
    //                foreach (var item in (IList)val)
    //                {
    //                    list.Add(item);
    //                }
    //                ((PropertyInfo)member).SetValue(baseObj, list, null);
    //            }
    //            if (((PropertyInfo)member).CanWrite)
    //                ((PropertyInfo)member).SetValue(derivedObj, val);
    //        }
    //    }
    //    return derivedObj;
    //}
}
