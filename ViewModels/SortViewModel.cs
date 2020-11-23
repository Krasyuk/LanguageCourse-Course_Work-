using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCourses.ViewModels
{
    public enum SortState
    {
        //Courses
        CoursesNameAsc,
        CoursesNameDesc,
        CoursesTrainingProgramAsc,
        CoursesTrainingProgramDesc,
        CoursesDescriptionAsc,
        CoursesDescriptionDesc,
        CoursesIntensityOfClassesAsc,
        CoursesIntensityOfClassesDesc,
        CoursesGroupSizeAsc,
        CoursesGroupSizeDesc,
        CoursesFreePlacesAsc,
        CoursesFreePlacesDesc,
        CoursesNumberOfHoursAsc,
        CoursesNumberOfHoursDesc,
        CoursesCostAsc,
        CoursesCostDesc,
        CoursesTeacherIdAsc,
        CoursesTeacherIdDesc,

        //Listeners
        ListinersNameAsc,
        ListinersNameDesc,
        ListinersSurnameAsc,
        ListinersSurnameDesc,
        ListinersMiddleNameAsc,
        ListinersMiddleNameDesc,
        ListinersDateOfBirthAsc,
        ListinersDateOfBirthDesc,
        ListinersAddressAsc,
        ListinersAddressDesc,
        ListinersPhoneAsc,
        ListinersPhoneDesc,
        ListinersPassportDataAsc,
        ListinersPassportDataDesc,

        //Payments
        PaymentsNameOfCoursesAsc,
        PaymentsNameOfCoursesDesc,
        PaymentsDateAsc,
        PaymentsDateDesc,
        PaymentsSumAsc,
        PaymentsSumDesc,
        PaymentsListenerIdAsc,
        PaymentsListenerIdDesc,
        PaymentsCourseIdAsc,
        PaymentsCourseIdDesc,

        //Teachers
        TeachersNameAsc,
        TeachersNameDesc,
        TeachersSurNameAsc,
        TeachersSurNameDesc,
        TeachersMiddleNameAsc,
        TeachersMiddleNameDesc,

    }
    public class SortViewModel
    {
        public SortState CurrentState { get; set; }
        //Courses
        public SortState CoursesName { get; set; }
        public SortState CoursesTrainingProgram { get; set; }
        public SortState CoursesDescription { get; set; }
        public SortState CoursesIntensityOfClasses { get; set; }
        public SortState CoursesGroupSize { get; set; }
        public SortState CoursesFreePlaces { get; set; }
        public SortState CoursesNumberOfHours { get; set; }
        public SortState CoursesCost { get; set; }
        public SortState CoursesTeacherId { get; set; }

        //Listeners
        public SortState ListinersName { get; set; }
        public SortState ListinersSurname { get; set; }
        public SortState ListinersMiddleName { get; set; }
        public SortState ListinersDateOfBirth { get; set; }
        public SortState ListinersAddress { get; set; }
        public SortState ListinersPhone { get; set; }
        public SortState ListinersPassportData { get; set; }

        //Payments
        public SortState PaymentsNameOfCourses { get; set; }
        public SortState PaymentsDate { get; set; }
        public SortState PaymentsSum { get; set; }
        public SortState PaymentsListenerId { get; set; }
        public SortState PaymentsCourseId { get; set; }

        //Teachers
        public SortState TeachersName { get; set; }
        public SortState TeachersSurName { get; set; }
        public SortState TeachersMiddleName { get; set; }

        public SortViewModel(SortState state)
        {
            CoursesName = state == SortState.CoursesNameAsc ? SortState.CoursesNameDesc : SortState.CoursesNameAsc;
            CoursesTrainingProgram = state == SortState.CoursesTrainingProgramAsc ? SortState.CoursesTrainingProgramDesc : SortState.CoursesTrainingProgramAsc;
            CoursesDescription = state == SortState.CoursesDescriptionAsc ? SortState.CoursesDescriptionDesc : SortState.CoursesDescriptionAsc;
            CoursesIntensityOfClasses = state == SortState.CoursesIntensityOfClassesAsc ? SortState.CoursesIntensityOfClassesDesc : SortState.CoursesIntensityOfClassesAsc;
            CoursesGroupSize = state == SortState.CoursesGroupSizeAsc ? SortState.CoursesGroupSizeDesc : SortState.CoursesGroupSizeAsc;
            CoursesFreePlaces = state == SortState.CoursesFreePlacesAsc ? SortState.CoursesFreePlacesDesc : SortState.CoursesFreePlacesAsc;
            CoursesNumberOfHours = state == SortState.CoursesNumberOfHoursAsc ? SortState.CoursesNumberOfHoursDesc : SortState.CoursesNumberOfHoursAsc;
            CoursesCost = state == SortState.CoursesCostAsc ? SortState.CoursesCostDesc : SortState.CoursesCostAsc;
            CoursesTeacherId = state == SortState.CoursesTeacherIdAsc ? SortState.CoursesTeacherIdDesc : SortState.CoursesTeacherIdAsc;

            ListinersName = state == SortState.ListinersNameAsc ? SortState.ListinersNameDesc : SortState.ListinersNameAsc;
            ListinersSurname = state == SortState.ListinersSurnameAsc ? SortState.ListinersSurnameDesc : SortState.ListinersSurnameAsc;
            ListinersMiddleName = state == SortState.ListinersMiddleNameAsc ? SortState.ListinersMiddleNameDesc : SortState.ListinersMiddleNameAsc;
            ListinersDateOfBirth = state == SortState.ListinersDateOfBirthAsc ? SortState.ListinersDateOfBirthDesc : SortState.ListinersDateOfBirthAsc;
            ListinersAddress = state == SortState.ListinersAddressAsc ? SortState.ListinersAddressDesc : SortState.ListinersAddressAsc;
            ListinersPhone = state == SortState.ListinersPhoneAsc ? SortState.ListinersPhoneDesc : SortState.ListinersPhoneAsc;
            ListinersPassportData = state == SortState.ListinersPassportDataAsc ? SortState.ListinersPassportDataDesc : SortState.ListinersPassportDataAsc;

            PaymentsNameOfCourses = state == SortState.PaymentsNameOfCoursesAsc ? SortState.PaymentsNameOfCoursesDesc : SortState.PaymentsNameOfCoursesAsc;
            PaymentsDate = state == SortState.PaymentsDateAsc ? SortState.PaymentsDateDesc : SortState.PaymentsDateAsc;
            PaymentsSum = state == SortState.PaymentsSumAsc ? SortState.PaymentsSumDesc : SortState.PaymentsSumAsc;
            PaymentsListenerId = state == SortState.PaymentsListenerIdAsc ? SortState.PaymentsListenerIdDesc : SortState.PaymentsListenerIdAsc;
            PaymentsCourseId = state == SortState.PaymentsCourseIdAsc ? SortState.PaymentsCourseIdDesc : SortState.PaymentsCourseIdAsc;

            TeachersName = state == SortState.TeachersNameAsc ? SortState.TeachersNameDesc : SortState.TeachersNameAsc;
            TeachersSurName = state == SortState.TeachersSurNameAsc ? SortState.TeachersSurNameDesc : SortState.TeachersSurNameAsc;
            TeachersMiddleName = state == SortState.TeachersMiddleNameAsc ? SortState.TeachersMiddleNameDesc : SortState.TeachersMiddleNameAsc;
        }




    }
}
