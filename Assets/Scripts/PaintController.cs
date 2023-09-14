using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class PaintController : MonoBehaviour
{
    [SerializeField]
    private RawImage m_image = null;

    private Texture2D m_texture = null;

    [SerializeField]
    private int penWidth = 4;

    [SerializeField]
    private int penHight = 4;

    [SerializeField]
    private int eraserWidth = 50;

    [SerializeField]
    private int eraserHight = 50;


    [SerializeField]
    private int m_width = 4;

    [SerializeField]
    private int m_height = 4;

    private Vector2 m_prePos;
    private Vector2 m_TouchPos;

    private float m_clickTime, m_preClickTime;

    Color currentColor = Color.black;

    [SerializeField] Texture2D penCursor;
    [SerializeField] Texture2D eraserCursor;

    public void OnDrag(BaseEventData arg) //����`��
    {
        PointerEventData _event = arg as PointerEventData; //�^�b�`�̏��擾

        // ������Ă���Ƃ��̏���
        m_TouchPos = _event.position; //���݂̃|�C���^�̍��W
        m_clickTime = _event.clickTime; //�Ō�ɃN���b�N�C�x���g�����M���ꂽ���Ԃ��擾

        float disTime = m_clickTime - m_preClickTime; //�O��̃N���b�N�C�x���g�Ƃ̎���

        int width = m_width;  //�y���̑���(�s�N�Z��)
        int height = m_height; //�y���̑���(�s�N�Z��)

        var dir = m_prePos - m_TouchPos; //���O�̃^�b�`���W�Ƃ̍�
        if (disTime > 0.01) dir = new Vector2(0, 0); //0.1�b�ȏ�Ԋu����������^�b�`���W�̍���0�ɂ���

        var dist = (int)dir.magnitude; //�^�b�`���W�x�N�g���̐�Βl

        dir = dir.normalized; //���K��

        //�w��̃y���̑���(�s�N�Z��)�ŁA�O��̃^�b�`���W���獡��̃^�b�`���W�܂œh��Ԃ�
        for (int d = 0; d < dist; ++d)
        {
            var p_pos = m_TouchPos + dir * d; //paint position
            p_pos.y -= height / 2.0f;
            p_pos.x -= width / 2.0f;
            for (int h = 0; h < height; ++h)
            {
                int y = (int)(p_pos.y + h);
                if (y < 0 || y > m_texture.height) continue; //�^�b�`���W���e�N�X�`���̊O�̏ꍇ�A�`�揈�����s��Ȃ�

                for (int w = 0; w < width; ++w)
                {
                    int x = (int)(p_pos.x + w);
                    if (x >= 0 && x <= m_texture.width)
                    {
                        m_texture.SetPixel(x, y, currentColor); //����`��
                    }
                }
            }
        }
        m_texture.Apply();
        m_prePos = m_TouchPos;
        m_preClickTime = m_clickTime;
    }

    public void OnTap(BaseEventData arg) //�_��`��
    {
        PointerEventData _event = arg as PointerEventData; //�^�b�`�̏��擾

        // ������Ă���Ƃ��̏���
        m_TouchPos = _event.position; //���݂̃|�C���^�̍��W

        int width = m_width;  //�y���̑���(�s�N�Z��)
        int height = m_height; //�y���̑���(�s�N�Z��)

        var p_pos = m_TouchPos; //paint position
        p_pos.y -= height / 2.0f;
        p_pos.x -= width / 2.0f;


        for (int h = 0; h < height; ++h)
        {
            int y = (int)(p_pos.y + h);
            if (y < 0 || y > m_texture.height) continue; //�^�b�`���W���e�N�X�`���̊O�̏ꍇ�A�`�揈�����s��Ȃ�
            for (int w = 0; w < width; ++w)
            {
                int x = (int)(p_pos.x + w);
                if (x >= 0 && x <= m_texture.width)
                {
                    m_texture.SetPixel(x, y, currentColor); //�_��`��
                }
            }
        }
        m_texture.Apply();
    }

    private void Start()
    {
        var rect = m_image.gameObject.GetComponent<RectTransform>().rect;
        m_texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);

        //���̍s�ǉ��i2021/10/21�j
        WhiteTexture((int)rect.width, (int)rect.height);

        m_image.texture = m_texture;

        Cursor.SetCursor(penCursor, Vector2.zero, CursorMode.Auto);
    }

    //���̊֐���ǉ��i2021/10/21�j
    //�e�N�X�`���𔒐F�ɂ���֐�
    private void WhiteTexture(int width, int height)
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                m_texture.SetPixel(w, h, Color.white);
            }
        }
        m_texture.Apply();
    }

    public void SetCurrentColor(string hexColor)
    {
        ColorUtility.TryParseHtmlString(hexColor, out currentColor);
        m_width = penWidth;
        m_height = penHight;
        Cursor.SetCursor(penCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetEraser()
    {
        currentColor = Color.white;
        m_width = eraserWidth;
        m_height = eraserHight;
        Cursor.SetCursor(eraserCursor, Vector2.zero, CursorMode.Auto);
    }

    public void Save()
    {
        string path = UnityEngine.Application.dataPath + "/" + "test.png";

        Debug.Log(path);

        byte[] png = m_texture.EncodeToPNG();

        File.WriteAllBytes(path, png);
    }

}
