using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using ApiHandler;
using SQLiteHandler;
using JwtHandler;

public class PatientSelection : MonoBehaviour
{

    private string connectionPath;

    public TMP_Text currentUserText;
    public TMP_Text currentUserEmailText;
    //Avatar
    public Image avatarImage;
    //TMP_Dropdown Entities
    public TMP_Dropdown entitiesDropdown;
    private List<string> entitiesIds = new List<string>();

    public GameObject patientSelectionPanel;

    // TMP_Dropdown Patients
    public TMP_Dropdown patientsDropdown;
    private List<string> admissionsIds = new List<string>();
    private List<AdmissionData> admissions = new List<AdmissionData>();

    public GameObject patientInfoPanel;

    public Image patientAvatarImage;
    public TMP_Text patientNameText;
    public TMP_Text patientEmailText;
    public TMP_Text patientGovIdText;
    public TMP_Text companionNameText;
    public TMP_Text companionPhoneText;
    public TMP_Text companionParentescoText;

    public GameObject playButton;

    void Awake()
    {
        connectionPath = "URI=file:" + Application.persistentDataPath + "/";
    }

    public void OnPlayButtonClicked()
    {
        AccessTokenManager.Instance.AdmissionId = admissionsIds[patientsDropdown.value];
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    // Start is called before the first frame update
    void Start()
    {
        playButton.SetActive(false);
        patientSelectionPanel.SetActive(false);
        patientInfoPanel.SetActive(false);
        JwtPayload payload = Jwt.GetPayload(AccessTokenManager.Instance.AccessToken);
        currentUserText.text = "Nombre: " + payload.names;
        currentUserEmailText.text = "Email: " + payload.email;
        //Avatar
        // call ApiRequest GetAvatar
        StartCoroutine(ApiRequest.GetAvatar(payload.avatar, (Sprite result) => {
            avatarImage.sprite = result;
        }, (string error) => {
            Debug.Log("Error: " + error);
        }));

        StartCoroutine(ApiRequest.GetEntities(AccessTokenManager.Instance.AccessToken, (List<EntityData> result) => {
            entitiesDropdown.ClearOptions();
            foreach (EntityData entity in result)
            {
                entitiesDropdown.options.Add(new TMP_Dropdown.OptionData(entity.ent_nombre));
                entitiesIds.Add(entity.id);
            }
            entitiesDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
        }, (string error) => {
            Debug.Log("Error: " + error);
        }));
    }

    private void DropdownValueChanged()
    {
        playButton.SetActive(false);
        admissionsIds.Clear();
        admissions.Clear();
        patientInfoPanel.SetActive(false);
        patientSelectionPanel.SetActive(false);
        string entityName = entitiesDropdown.options[entitiesDropdown.value].text;
        string entityId = entitiesIds[entitiesDropdown.value];

        Debug.Log("Entity name: " + entityName + ", Entity id: " + entityId);

        StartCoroutine(ApiRequest.GetAdmissions(AccessTokenManager.Instance.AccessToken, entityId, (List<AdmissionData> result) => {
            patientsDropdown.ClearOptions();
            foreach (AdmissionData admission in result)
            {
                patientsDropdown.options.Add(new TMP_Dropdown.OptionData(admission.patient.profile.pro_nombre + " " + admission.patient.profile.pro_apelli));
                admissionsIds.Add(admission.id);
                admissions.Add(admission);
            }
            patientsDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged2(); });
            patientSelectionPanel.SetActive(true);
        }, (string error) => {
            Debug.Log("Error: " + error);
        }));
    }

    private void DropdownValueChanged2()
    {
        playButton.SetActive(true);
        patientInfoPanel.SetActive(false);
        string admissionId = admissionsIds[patientsDropdown.value];
        AdmissionData admission = admissions[patientsDropdown.value];
        UserData patient = admission.patient;
        Profile patientProfile = patient.profile;
        Companion companion = admission.adm_compan;
        Profile companionProfile = companion.com_profil;

        Debug.Log("Admission id: " + admissionId);

        StartCoroutine(ApiRequest.GetAvatar(patientProfile.pro_avatar, (Sprite result) => {
            patientAvatarImage.sprite = result;
        }, (string error) => {
            Debug.Log("Error: " + error);
        }));

        patientNameText.text = patientProfile.pro_nombre + " " + patientProfile.pro_apelli;
        patientEmailText.text = patient.email;
        patientGovIdText.text = patientProfile.pro_tipide + patientProfile.pro_numide;
        companionNameText.text = companionProfile.pro_nombre + " " + companionProfile.pro_apelli;
        companionPhoneText.text = "+" + companionProfile.pro_celpai + companionProfile.pro_celula;
        companionParentescoText.text = companion.com_parent;
        patientInfoPanel.SetActive(true);
    }
}
